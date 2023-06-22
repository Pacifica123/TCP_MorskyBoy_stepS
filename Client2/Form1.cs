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

namespace Client2
{
    public partial class Form1 : Form
    {
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
        private int currentShipSize; // Размер текущего корабля
        private int shipCounter; // Счетчик установленных кораблей
        private bool isPlacingShip; // Флаг режима расстановки корабля
        private Ship currentShip; // Текущий корабль
        static string ipServer;
        List<Ship> Ships_buffer = new List<Ship>(); // Буферный список для отправки кораблей на сервер
        private List<Ship> shipBuffer;  // Буферный список кораблей для размещения на поле

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
        private bool IsValidIpAddress(string ipAddress)
        {
            string pattern = @"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$";
            return Regex.IsMatch(ipAddress, pattern);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            // Отправляем запрос на отключение клиента
            // Отправить запрос на отключение клиента на сервер
            string disconnectMessage = "disconnect"; // Здесь может быть любое сообщение, которое сервер будет распознавать как запрос на отключение
            SendMessageToServer(disconnectMessage);
        }

        // Отправка сообщения на сервер
        private void SendMessageToServer(string message)
        {
            using (TcpClient client = new TcpClient(ipServer, 8888))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.UTF8.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                }
            }
        }
        //===============================
        private void Form1_Load(object sender, EventArgs e)
        {
            // Конфигурируем поле
            YourSea.DefaultCellStyle.BackColor = Color.White; // Задаем цвет фона ячеек
            YourSea.DefaultCellStyle.SelectionBackColor = Color.Blue; // Задаем цвет выделенных ячеек
            YourSea.DefaultCellStyle.SelectionForeColor = Color.White; // Задаем цвет текста выделенных ячеек
            YourSea.RowCount = 10;
            YourSea.ColumnCount = 10;
            YourSea.ReadOnly = true;
            
        }

        private void YourSea_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {

            if (currentShip != null)
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
        private void UpdateButtonStates()
        {
            // Проверяем, есть ли расстановка кораблей
            bool hasShipPlacement = Ships_buffer.Count > 0;

            // Обновляем состояние кнопок
            btnConfirm.Enabled = hasShipPlacement;
            btnCancel.Enabled = hasShipPlacement;
            btnDisconnect.Enabled = !hasShipPlacement;
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
        }

    }
}
