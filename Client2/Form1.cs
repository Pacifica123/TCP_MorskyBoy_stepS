using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Server2.Engine;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace Client2
{
    public partial class Form1 : Form
    {
        #region ИНИЦИАЛИЗАЦИЯ
        public Form1()
        {
            InitializeComponent();
            // Инициализация буферного списка кораблей
            shipBuffer = new List<Ship>();
            shipBuffer.Add(new Ship(4));  // 1 корабль типа 4
            shipBuffer.Add(new Ship(3));  // 2 корабля типа 3
            shipBuffer.Add(new Ship(3));
            shipBuffer.Add(new Ship(2));  // 3 корабля типа 2
            shipBuffer.Add(new Ship(2));
            shipBuffer.Add(new Ship(2));
            shipBuffer.Add(new Ship(1));  // 4 корабля типа 1
            shipBuffer.Add(new Ship(1));
            shipBuffer.Add(new Ship(1));
            shipBuffer.Add(new Ship(1));
            currentShip = shipBuffer.Last();
        }

        // Глобальные переменные
        private bool isGameStarted = false;
        private bool isUserFieldEditable = true;
        private Ship currentShip; // Текущий корабль
        static string ipServer; // Конечная точка подключения
        List<Ship> Ships_buffer = new List<Ship>(); // Буферный список для отправки кораблей на сервер
        private List<Ship> shipBuffer;  // Буферный список кораблей для размещения на поле
        IPAddress MyIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList
            .FirstOrDefault(addr => addr.AddressFamily==AddressFamily.InterNetwork);

        private void Form1_Load(object sender, EventArgs e)
        {
            // Конфигурируем поле
            YourSea.DefaultCellStyle.BackColor = Color.White; // Задаем цвет фона ячеек
            YourSea.DefaultCellStyle.SelectionBackColor = Color.Blue; // Задаем цвет выделенных ячеек
            YourSea.DefaultCellStyle.SelectionForeColor = Color.White; // Задаем цвет текста выделенных ячеек
            YourSea.RowCount = 10;
            YourSea.ColumnCount = 10;
            YourSea.ReadOnly = true;
            // подгон высоты строк
            //YourSea.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            int availableHeight = YourSea.ClientSize.Height;
            int rowCount = YourSea.RowCount;

            int rowHeight = availableHeight / rowCount;

            for (int i = 0; i < rowCount; i++)
            {
                
                YourSea.Rows[i].Height = rowHeight;
            }
        }
        #endregion

        #region СЕТЬ
        //================================СЕТЬ
        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            string ipAddress = txtIpAddress.Text.Trim();

            if (IsValidIpAddress(ipAddress))
            {
                try
                {
                    using (TcpClient client = new TcpClient())
                    {
                        client.Connect(ipAddress, 8888); 

                        using (NetworkStream stream = client.GetStream())
                        {
                            byte[] data = Encoding.UTF8.GetBytes("test");
                            stream.Write(data, 0, data.Length);

                            data = new byte[256];
                            int bytesRead = stream.Read(data, 0, data.Length);
                            string response = Encoding.UTF8.GetString(data, 0, bytesRead);

                            txtIpAddress.Text = response;

                            ipServer = ipAddress;
                        }
                    }
                    GetGameStateMotor();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при подключении к серверу: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите действительный IP-адрес", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// Механизм по периодическому спросу состояния игры у сервера
        /// </summary>
        private async void GetGameStateMotor()
        {
                await Task.Delay(3000);
                SendMessageToServer("get_state");

        }

        private bool IsValidIpAddress(string ipAddress)
        {
            string pattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            return Regex.IsMatch(ipAddress, pattern);
        }

        

        // Отправка сообщения на сервер
        private async void SendMessageToServer(string message)
        {
            using (TcpClient client = new TcpClient(ipServer, 8888))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    await Task.Run(() => stream.Write(data, 0, data.Length));
                    // Получение ответа от сервера
                    string response = await Task.Run(() => ReceiveMessageFromServer(stream));

                    // Обработка ответа от сервера
                    await HandleServerResponse(response); //(там итак await внутри)
                }
            }
        }
        // Ожидание ответа от сервера
        private async Task WaitForServerResponse()
        {
            // Задержка в 5 секунд
            await Task.Delay(5000);
            // Отправка запроса на сервер в зависимости от состояния игры
            if (!isGameStarted)
                SendMessageToServer("YouReady?");
            // тут должен был быть else с отправкой OpponentAlreadyAttacked? но потом все пошло немного по другому
        }   

        private string ReceiveMessageFromServer(NetworkStream stream)
        {
            byte[] buffer = new byte[1024]; // Буфер для приема данных
            StringBuilder stringBuilder = new StringBuilder(); // Строка для сбора принятых данных

            // Прием данных от сервера
            //собираем ответ с Сервака
            byte[] responseBytes = new byte[1024];
            int bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
            string response = Encoding.ASCII.GetString(responseBytes, 0, bytesRead);
            return response;
        }

        /// <summary>
        /// Обработка ответа от сервера
        /// </summary>
        /// <param name="response">ответ сервера</param>
        /// <returns>Действие в зависимости от ответа сервера</returns>
        private async Task HandleServerResponse(string response)
        {
            await Task.Run(() =>
            {
                ProcessServerResponse(response);
            });
        }
        /// <summary>
        /// Механизм обработки сервера (ядро HandleServerRespons-а)
        /// </summary>
        /// <param name="response">ответ сервера</param>
        private void ProcessServerResponse(string response)
        {
            switch (response)
            {
                case "DontSecond":
                    MessageBox.Show("Подождите второго игрока.");
                    // Ожидание ответа от сервера
                    WaitForServerResponse();
                    break;
                case "NotReady":
                    MessageBox.Show("Пока что 2й игрок не расставил свои корабли");
                    // Ожидание ответа от сервера
                    WaitForServerResponse();
                    break;
                case string result when result.StartsWith("GameStarted"):
                    ClientGameTransformation(result.Substring("GameStarted:".Length));
                    break;
                case string result when result.StartsWith("AttackResult"):
                    ProcessAttackResult(result.Substring("AttackResult".Length));
                    break;
                //case string result when result.StartsWith("OpponentAttackResult"):
                //    ProcessOpponentAttackResult(result);
                //    break;
                case "NotAlready":
                    // снова ждем но уже когда сходит пртивник
                    ToolTip waiting = new ToolTip();
                    waiting.Show("Ждем ответа оппонента...", OpponentSea);
                    WaitForServerResponse();
                    break;
                case string result when result.StartsWith("STATE"):
                    ProcessGameState(result.Substring("STATE".Length));
                    break;
                case "NotYourTurn":
                    MessageBox.Show("Сейчас не ваш ход!");
                    break;
                default:
                    // Обработка неизвестного ответа
                    MessageBox.Show("Что-то пошло не так!");
                    break;
            }
        }
        /// <summary>
        /// Актуализирует клиента под состояние игры на сервер
        /// </summary>
        /// <param name="GameStateJSON"></param>
        private void ProcessGameState(string GameStateJSON)
        {
            Player player = JsonConvert.DeserializeObject<Player>(GameStateJSON);
            Game currentGameState = JsonConvert.DeserializeObject<Game>(GameStateJSON, new JsonSerializerSettings());
            if (currentGameState.Players.Count == 2)
                OpponentSea.Enabled = currentGameState.CurrentPlayer.PlayerId == MyIP.ToString(); //разрешено ли ходить пользователю
            if (currentGameState.LastTurn != null)
            {
                if (currentGameState.LastTurn.AtackedPlayer.PlayerId == MyIP.ToString())
                {
                    Turn last = currentGameState.LastTurn;
                    ProcessOpponentAttackResult(last.X, last.Y, last.resultForNextPlayer);
                }
            }
            

            GetGameStateMotor();
        }

        //===============================
        #endregion


        #region ВНЕШКА
        //--------------------------------------------------
        // КЛИКИ
        //--------------------------------------------------
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // Отправляем запрос на отключение клиента
            // Отправить запрос на отключение клиента на сервер
            string disconnectMessage = "disconnect"; // Здесь может быть любое сообщение, которое сервер будет распознавать как запрос на отключение
            SendMessageToServer(disconnectMessage);
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            // Сериализация списка кораблей в JSON
            string shipBufferJson = JsonConvert.SerializeObject(Ships_buffer);

            // Добавление кодового слова для определения расстановки
            string message = "placement:" + shipBufferJson;

            // Отправка сообщения на сервер
            SendMessageToServer(message);


        }
        private void YourSea_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (shipBuffer.Count == 0)
            {
                UpdateButtonStates();
                return; //расстановка завершена
            }
            // Проверяем, выбрана ли какая-либо радиокнопка
            if (rbHorisontal.Checked || rbVertical.Checked)
            {
                // Получаем выбранную ячейку
                DataGridViewCell cell = YourSea.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Перемещаем текущий корабль на новые координаты
                MoveTo(currentShip, cell.RowIndex, cell.ColumnIndex);

                // Отобразить изменения на поле
                UpdateGridView();

                // Удалить выделение ячейки
                YourSea.ClearSelection();
            }
            else
            {
                MessageBox.Show("Выберите ориентацию корабля (горизонтальную или вертикальную).", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void btnConfirm_Click(object sender, EventArgs e)
        {

            if (currentShip != null)
            {
                if (!IsShipColliding(currentShip))
                {
                
                    // Добавить текущий корабль в буфер и удалить его из списка кораблей
                    Ships_buffer.Add(currentShip);
                    shipBuffer.Remove(currentShip);
                    //currentShip = null;

                    // Окрасить ячейки корабля в другой цвет
                    foreach (SeaCell cell in currentShip.ShipCells)
                    {
                        YourSea.Rows[cell.Y].Cells[cell.X].Style.BackColor = Color.Gray;
                    }

                    // Обновить доступность кнопок
                    UpdateButtonStates();

                    // Очистить выделение ячейки
                    YourSea.ClearSelection();

                    // Следующий корабль
                    if (shipBuffer.Count > 0)
                        currentShip = shipBuffer.Last();
               
                }
                else
                {
                    MessageBox.Show("Корабль не может касаться других кораблей!");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (Ships_buffer.Count > 0)
            {
                // Получение последней расстановки корабля из списка и ее удаление
                Ship lastShip = Ships_buffer.Last();
                Ships_buffer.Remove(lastShip);
                shipBuffer.Add(lastShip);

                // Окрашиваем ячейки, соответствующие удаленному кораблю, в исходный цвет
                foreach (SeaCell cell in lastShip.ShipCells)
                {
                    YourSea.Rows[cell.Y].Cells[cell.X].Style.BackColor = Color.White;
                }
                currentShip = lastShip;
            }

            // Обновление доступности кнопок
            UpdateButtonStates();
        }
        private void OpponentSea_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (isGameStarted)
            {
                AttackOpponentCell(e.ColumnIndex, e.RowIndex);
               
            }
        }
        //--------------------------------------------------
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
        //--------------------------------------------------
        private void MoveTo(Ship currentShip, int rowIndex, int columnIndex)
        {
            // Очистить предыдущие координаты корабля
            foreach (SeaCell shipCell in currentShip.ShipCells)
            {
                YourSea.Rows[shipCell.Y].Cells[shipCell.X].Style.BackColor = Color.White;
            }

            // Обновить координаты корабля
            int size = currentShip.Type;
            bool isHorizontal = rbHorisontal.Checked; // Получить выбранное значение из RadioButton
            int endRow = isHorizontal ? rowIndex : rowIndex + size - 1;
            int endColumn = isHorizontal ? columnIndex + size - 1 : columnIndex;

            // Проверить, что новые координаты не выходят за границы поля
            if (endRow < YourSea.RowCount && endColumn < YourSea.ColumnCount)
            {
                // Обновить координаты корабля
                currentShip.ShipCells.Clear();
                for (int i = rowIndex; i <= endRow; i++)
                {
                    for (int j = columnIndex; j <= endColumn; j++)
                    {
                        if (YourSea.Rows[i].Cells[j].Style.BackColor != Color.Gray)
                        {
                            YourSea.Rows[i].Cells[j].Style.BackColor = Color.LightGray;
                            currentShip.ShipCells.Add(new SeaCell(j, i, SeaCell.CellState.OccupiedByShip));
                        }
                        
                    }
                }
            }
            // Проверяем, остались ли еще корабли в списке shipBuffer
            if (shipBuffer.Count > 0)
            {
                currentShip = shipBuffer.Last();
            }
            else
            {
                // Все корабли размещены
                MessageBox.Show("Все корабли размещены!");
            }
        }

        private void UpdateButtonStates()
        {
            // Проверяем, есть ли расстановка кораблей
            bool hasShipPlacement = shipBuffer.Count > 0;

            // Обновляем состояние кнопок
            btnConfirm.Enabled = hasShipPlacement;
            btnCancel.Enabled = hasShipPlacement;
            btnDisconnect.Enabled = hasShipPlacement;
        }

        private void UpdateGridView()
        {
            // Обновить отображение полей

            // Сначала очищаем все ячейки кроме утвержденных
            foreach (DataGridViewRow row in YourSea.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if(cell.Style.BackColor != Color.Gray)
                        cell.Style.BackColor = Color.White;
                }
            }

            // Затем изменяем цвет ячеек корабля
            if (currentShip != null)
            {
                foreach (SeaCell shipCell in currentShip.ShipCells)
                {
                    DataGridViewCell cell = YourSea.Rows[shipCell.Y].Cells[shipCell.X];
                    cell.Style.BackColor = Color.LightGray;
                }
            }
           // YourSea.Enabled = !isUserFieldEditable;
        }
        /// <summary>
        /// Этот корабль конфликтует хотя с одним из уже выставленных?
        /// </summary>
        /// <param name="ship"></param>
        /// <returns>true - конфликтует; false - все нормально</returns>
        private bool IsShipColliding(Ship ship)
        {
            foreach (Ship otherShip in Ships_buffer)
            {
                if (CompareShips(ship, otherShip))
                    return true;
            }

            return false;
        }
        /// <summary>
        /// Сравнивает расположение двух короблей относительно друг друга по клеточно, проверяя соприкасаются/пересекаются ли ячейки
        /// </summary>
        /// <param name="ship1"></param>
        /// <param name="ship2"></param>
        /// <returns>true - да, пересекаются/соприкасаются; false - нет, все нормально</returns>
        private bool CompareShips(Ship ship1, Ship ship2)
        {
            foreach (SeaCell cell1 in ship1.ShipCells)
            {
                foreach (SeaCell cell2 in ship2.ShipCells)
                {
                    // Проверяем, что ячейки кораблей пересекаются
                    if (Math.Abs(cell1.X - cell2.X) <= 1 && Math.Abs(cell1.Y - cell2.Y) <= 1)
                        return true;
                }
            }

            return false;
        }

        private void ClientGameTransformation(string WhosTurn)
        {
            isGameStarted = true;
            isUserFieldEditable = false;
            MessageBox.Show("Игра началась");

            // Включить и отрисовать поле противника
            InitOpponentSea();

            if (WhosTurn == MyIP.ToString())
            {
                MessageBox.Show("Право первого хода...Досталось вам!");
                //игрок атакует поле противника - это событие Дабл-клика "OpponentSea_CellDoubleClick"
            }
            else
            {
                MessageBox.Show("Право первого хода...Досталось оппоненту.");
                SendMessageToServer("OpponentAlreadyAtacked?");
            }
        }
        #region РЕЖИМ АТАКИ
        private void InitOpponentSea()
        {
            OpponentSea.Enabled = true;
        }
        private async void AttackOpponentCell(int selectedCellX, int selectedCellY)
        {
            // Отправка координат на сервер
            string message = $"Attack:{selectedCellX},{selectedCellY}";
            SendMessageToServer(message);
        }
        /// <summary>
        /// Результат нашей атаки
        /// </summary>
        /// <param name="result"></param>
        private void ProcessAttackResult(string result)
        {
            int attackedCellX, attackedCellY;
            bool isHit;

            // Разбор ответа сервера
            string[] tokens = result.Split(',');
            attackedCellX = int.Parse(tokens[1]);
            attackedCellY = int.Parse(tokens[2]);
            isHit = tokens[0] == "you_shot";

            OpponentSea.Enabled = isHit;

            // Окрашивание клетки на поле пользователя или противника в зависимости от результата атаки
            if (isHit)
            {
                DataGridViewCell cell = OpponentSea.Rows[attackedCellY].Cells[attackedCellX];
                cell.Style.BackColor = Color.Red;
                cell.Value = "X";
            }
            else
            {
                DataGridViewCell cell = OpponentSea.Rows[attackedCellY].Cells[attackedCellX];
                cell.Style.BackColor = Color.DarkGray;
                cell.Value = "*";
            }

        }
        /// <summary>
        /// результат атаки противника по нашему полю
        /// </summary>
        /// <param name="result"></param>
        private void ProcessOpponentAttackResult(string result)
        {
            result = result.Substring("OpponentAttackResult:".Length);
            int attackedCellX, attackedCellY;
            bool isHit;

            // Разбор ответа сервера
            string[] tokens = result.Split(',');
            attackedCellX = int.Parse(tokens[1]);
            attackedCellY = int.Parse(tokens[2]);
            isHit = tokens[0] == /*"you_fail" || tokens[0] ==*/ "opponent_shot";

            // Окрашивание клетки на поле пользователя в зависимости от результата атаки
            if (isHit)
            {
                DataGridViewCell cell = YourSea.Rows[attackedCellY].Cells[attackedCellX];
                cell.Style.BackColor = Color.Black;

            }
            else
            {
                DataGridViewCell cell = YourSea.Rows[attackedCellY].Cells[attackedCellX];
                cell.Style.BackColor = Color.Blue;
            }
        }
        private void ProcessOpponentAttackResult(int X, int Y, string result)
        {

            bool isHit = result == "opponent_shot";

            // Окрашивание клетки на поле пользователя в зависимости от результата атаки
            if (isHit)
            {
                DataGridViewCell cell = YourSea.Rows[Y].Cells[X];
                cell.Style.BackColor = Color.Red;
                cell.Value = "X";
            }
            else
            {
                DataGridViewCell cell = YourSea.Rows[Y].Cells[X];
                cell.Style.BackColor = Color.Aqua;

            }
        }

        #endregion

        #endregion


    }
}
