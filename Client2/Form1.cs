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
using ProtoBuf;

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
        Color defaultCell = Color.FromArgb(0x41, 0x80, 0x90); //Color.RoyalBlue;
        Color shipCell = Color.FromArgb(0x10, 0x44, 0x80);
        private bool isGameStarted = false;
        private bool isGameOver = false;
        private Ship currentShip; // Текущий корабль
        static string ipServer; // Конечная точка подключения
        List<Ship> Ships_buffer = new List<Ship>(); // Буферный список для отправки кораблей на сервер
        private List<Ship> shipBuffer;  // Буферный список кораблей для размещения на поле
        IPAddress MyIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList
            .FirstOrDefault(addr => addr.AddressFamily==AddressFamily.InterNetwork);

        private void Form1_Load(object sender, EventArgs e)
        {
            
            // Конфигурируем поле
            YourSea.DefaultCellStyle.BackColor = defaultCell; // Задаем цвет фона ячеек
            YourSea.DefaultCellStyle.SelectionBackColor = Color.LightBlue; // Задаем цвет выделенных ячеек
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
        
        /// <summary>
        /// Подключение к Серверу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckConnection_Click(object sender, EventArgs e)
        {
            if (isGameOver) return;
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
                            ProcessServerResponse(response, client);

                            ipServer = ipAddress;
                        }
                    }
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
            if (isGameOver)
                return; 
           
            await Task.Delay(1000);
            SendMessageToServer("get_last");
        }
         
        
        /// <summary>
        /// проверка IP адресса на корректность
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private bool IsValidIpAddress(string ipAddress)
        {
            string pattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            return Regex.IsMatch(ipAddress, pattern);
        }

        

        /// <summary>
        /// отправка сообщения на сервер и обработка последующего ответа
        /// </summary>
        /// <param name="message"></param>
        private async void SendMessageToServer(string message)
        {
            if (ipServer == null || ipServer == "")
            {
                MessageBox.Show("Вы не подключились к серверу!");
                return;
            }
            if (isGameOver) return;
            if (!isGameOver)
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
                        if (isGameOver)
                        {
                            client.Client.Close();
                            return;
                        }
                        await HandleServerResponse(response, client); //(там итак await внутри)
                    }
                }
            }
        }

        
        /// <summary>
        /// Задача ожидания сервера когда не все подключились/расставили корабли
        /// </summary>
        /// <returns></returns>
        private async Task WaitForServerResponse()
        {
            if (isGameOver) return;
            // Задержка в 5 секунд
            await Task.Delay(5000);
            // Отправка запроса на сервер в зависимости от состояния игры
            if (!isGameStarted)
                SendMessageToServer("YouReady?");
        }   

        
        /// <summary>
        /// Получаем ответ с сервера
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>строка овтета от сервера</returns>
        private string ReceiveMessageFromServer(NetworkStream stream)
        {
            byte[] responseBytes = new byte[4096];
            int bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
            
            string response = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);
            if (response.Contains("STATE")) return Convert.ToBase64String(responseBytes);
            return response;
        }

        
        /// <summary>
        /// Обработка ответа от сервера
        /// </summary>
        /// <param name="response">ответ сервера</param>
        /// <returns>Действие в зависимости от ответа сервера</returns>
        private async Task HandleServerResponse(string response, TcpClient client)
        {
            if (isGameOver) return;
            if (!isGameOver)
            {
                await Task.Run(() =>
                {

                    ProcessServerResponse(response, client);
                });
            }
        }

        
        /// <summary>
        /// Механизм обработки сервера (ядро HandleServerRespons-а)
        /// </summary>
        /// <param name="response">ответ сервера</param>
        private void ProcessServerResponse(string response, TcpClient client)
        {
            if (isGameOver) return;
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
                case string result when result.Contains("LastTurn:"):
                    if (isGameOver) return;
                    ProcessLastTurn(result.Substring("LastTurn:".Length), client);
                    break;
                case "Good!":
                    break;
                case "":
                    break; //заглушка пока на закомментированные методы на Сервере
                default:
                    // Обработка неизвестного ответа
                    MessageBox.Show("Что-то пошло не так!");
                    break;
            }
        }

        
        /// <summary>
        /// Механизм обработки последнего хода и адаптации представления клиента под него
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="client"></param>
        private void ProcessLastTurn(string turn, TcpClient client)
        {
            if (isGameOver) return;
            Turn last = JsonConvert.DeserializeObject<Turn>(turn);

            if (last == null) { MessageBox.Show("Происходит какая-то ошибка!"); return; }
            if (last.resultForNextPlayer != null && last.resultForNextPlayer.StartsWith("WIN:"))
            {
                ProcessFinal(last.resultForNextPlayer.Substring("WIN:".Length), client);
                return;
            }
            if (last.AtackedPlayerID != null && last.AtackerID != null && (last.resultForNextPlayer != "" || last.resultForNextPlayer != "NotAlredy"))
            {
                // здесь resultForNextPlayer это противник - красим его поле
                if (last.AtackerID == MyIP.ToString()) ProcessAttackResult(last.X, last.Y, last.resultForNextPlayer);
                // здесь resultForNextPlayer - это мы - красим наше поле:
                if (last.AtackedPlayerID == MyIP.ToString()) ProcessOpponentAttackResult(last.X, last.Y, last.resultForNextPlayer);
                
            }

            if (isGameOver) return;
            GetGameStateMotor();
        }

        
        /// <summary>
        /// Вывод победителя, разрывание подключения
        /// </summary>
        /// <param name="winnerIP"></param>
        /// <param name="client"></param>
        private void ProcessFinal(string winnerIP, TcpClient client)
        {
            if (isGameOver) return;
            isGameOver = true;
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    this.Enabled = false;
                });
            }
            else this.Enabled = false;
            //string messagewinner;
            SendMessageToServer("disconnect");
            if (winnerIP == MyIP.ToString())
            {

                MessageBox.Show("Поздравляем!\nВы победили!"); 
                //messagewinner = "Поздравляем!\nВы победили!";
                    
                
            }
            else
            {

                MessageBox.Show("Игра окончена.\nВы проиграли =(");
                //messagewinner = "Игра окончена.\nВы проиграли =(";
                
               // Application.Exit();
            }
            
            //client.Client.Close();
            Task.Delay(5000);
            Application.Exit();
        }

        
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
            if (ipServer == null || ipServer == "" || txtIpAddress.Text != "Good!")
            {
                MessageBox.Show("Вы не подключились к серверу!");
                return;
            }
            if (shipBuffer.Count > 0)
            {
                MessageBox.Show("Вы не расставили все корабли!");
                return;
            }
            // Сериализация списка кораблей в JSON
            string shipBufferJson = JsonConvert.SerializeObject(Ships_buffer);

            // Добавление кодового слова для определения расстановки
            string message = "placement:" + shipBufferJson;

            // Отправка сообщения на сервер
            SendMessageToServer(message);
            btnSend.Enabled = false;
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
                        YourSea.Rows[cell.Y].Cells[cell.X].Style.BackColor = shipCell;
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
                    YourSea.Rows[cell.Y].Cells[cell.X].Style.BackColor = defaultCell;
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
                SendMessageToServer("get_last"); //внеочередной спрос для ускорения??
            }
        }
        private void btnRandomPlacement_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            // Очищаем поле перед расстановкой
            //YourSea.Rows.Clear();
           // YourSea.Refresh();

            // Расставляем корабли случайным образом
            List<Ship> shipBufferCopy = new List<Ship>(shipBuffer);
            foreach (Ship ship in shipBufferCopy)
            {
                bool placed = false;

                while (!placed)
                {
                    int x = random.Next(YourSea.ColumnCount);
                    int y = random.Next(YourSea.RowCount);
                    bool isVertical = random.Next(2) == 0;
                    SeaCell startCell = new SeaCell(x, y, SeaCell.CellState.OccupiedByShip);
                    ship.ShipCells = (List<SeaCell>)GetShipCells(ship, startCell, isVertical);

                    if (CanPlaceShip(ship, x, y, isVertical) && !IsShipColliding(ship))
                    {
                        PlaceShip(ship, x, y, isVertical);
                        placed = true;
                    }
                }
            }

            //// Обновляем состояние кнопок
            UpdateButtonStates();
        }
        //--------------------------------------------------
        // ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
        //--------------------------------------------------
        private void PlaceShip(Ship ship, int startX, int startY, bool isVertical)
        {
            // Добавляем корабль в буфер и удаляем его из списка кораблей
            Ships_buffer.Add(ship);
            shipBuffer.Remove(ship);
            // Окрашиваем ячейки корабля в другой цвет
            foreach (SeaCell cell in ship.ShipCells)
            {
                int x = startX + (cell.X-startX);
                int y = startY + (cell.Y-startY);
                YourSea.Rows[y].Cells[x].Style.BackColor = shipCell;
            }

            // Обновляем состояние кнопок
            UpdateButtonStates();
        }
        public bool CanPlaceShip(Ship ship, int startX, int startY, bool isVertical)
        {
            SeaCell startCell = new SeaCell(startX, startY, SeaCell.CellState.OccupiedByShip);
            // Проверяем, не выходит ли корабль за границы поля
            if (isVertical)
            {
                if (startCell.Y + ship.Type >= YourSea.RowCount ) 
                    return false;
            }
            else
            {
                if (startCell.X + ship.Type >= YourSea.ColumnCount )
                    return false;
            }

            // Проверяем, не перекрывает ли корабль другие корабли
            foreach (SeaCell cell in ship.ShipCells)
            {
                int x = startX + cell.X;
                int y = startY + cell.Y;

                if (x >= 0 && x < YourSea.ColumnCount && y >= 0 && y < YourSea.RowCount)
                {
                    if (YourSea.Rows[y].Cells[x].Style.BackColor == shipCell)
                        return false;
                }
            }

            return true;
        }
        private IEnumerable<SeaCell> GetShipCells(Ship ship, SeaCell startCell, bool isVertical)
        {
            List<SeaCell> cells = new List<SeaCell>();

            for (int i = 0; i < ship.Type; i++)
            {
                int x = startCell.X;
                int y = startCell.Y;

                if (isVertical)
                    y += i;
                else
                    x += i;

                cells.Add(new SeaCell(x, y, SeaCell.CellState.OccupiedByShip));
            }

            return cells;
        }
        private void MoveTo(Ship currentShip, int rowIndex, int columnIndex)
        {
            // Очистить предыдущие координаты корабля
            foreach (SeaCell shipCell in currentShip.ShipCells)
            {
                YourSea.Rows[shipCell.Y].Cells[shipCell.X].Style.BackColor = defaultCell;
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
                        if (YourSea.Rows[i].Cells[j].Style.BackColor != shipCell)
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
            btnRandomPlacement.Enabled = hasShipPlacement;
            //btnSend.Enabled = hasShipPlacement;
        }
        private void UpdateGridView()
        {
            // Сначала очищаем все ячейки кроме утвержденных
            foreach (DataGridViewRow row in YourSea.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if(cell.Style.BackColor != shipCell)
                        cell.Style.BackColor = defaultCell;
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


        /// <summary>
        /// Преобразует клиента когда оба игрока будут готовы к началу игры
        /// </summary>
        /// <param name="WhosTurn"></param>
        private void ClientGameTransformation(string WhosTurn)
        {
            isGameStarted = true;
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
                //SendMessageToServer("OpponentAlreadyAtacked?");
            }
            GetGameStateMotor();
        }
        
        #region РЕЖИМ АТАКИ
        private void InitOpponentSea()
        {
            if (OpponentSea.InvokeRequired)
            {
                OpponentSea.Invoke((MethodInvoker)delegate 
                { 
                    OpponentSea.DefaultCellStyle.BackColor = defaultCell;
                    OpponentSea.Enabled = true;
                    OpponentSea.ColumnCount = 10;
                    OpponentSea.RowCount = 10;
                    int availableHeight = OpponentSea.ClientSize.Height;
                    int rowCount = YourSea.RowCount;

                    int rowHeight = availableHeight / rowCount;

                    for (int i = 0; i < rowCount; i++)
                    {

                        OpponentSea.Rows[i].Height = rowHeight;
                    }
                });
                
            }
            else
            {
                OpponentSea.Enabled = true;
            }

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
        private void ProcessAttackResult(int X, int Y, string AtackResult)
        {
            int attackedCellX, attackedCellY;
            bool isHit;

            // Разбор ответа сервера
            //string[] tokens = result.Split(',');
            attackedCellX = X;
            attackedCellY = Y;
            isHit = AtackResult == "shot";

            if (OpponentSea.InvokeRequired)
            {
                OpponentSea.Invoke((MethodInvoker)delegate { OpponentSea.Enabled = isHit; });
            }
            else OpponentSea.Enabled = isHit;

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
        /// Результат атаки противника
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="result"></param>
        private void ProcessOpponentAttackResult(int X, int Y, string result)
        {

            bool isHit = result == "shot";

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
                cell.Style.BackColor = Color.DarkGray;

                if (OpponentSea.InvokeRequired)
                {
                    OpponentSea.Invoke((MethodInvoker)delegate
                    { OpponentSea.Enabled = true; });

                }
                else
                {
                    OpponentSea.Enabled = true;
                }
            }
            
        }


        #endregion

        #endregion

    }
}
