using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
//using System.Text.Json;
using System.Xml.XPath;
using System.IO;
using System.Net;

namespace Client.forms
{
    public partial class Main : Form
    {
        public List<Point> selectedCellsCoordlist;
        private TcpClient client;
        private NetworkStream stream;
        private string ipServer;
        private List<Ship> shipSET;
        

        public Main()
        {
            InitializeComponent();
            InitCast(dgvYourSea);
            selectedCellsCoordlist = new List<Point>();
            shipSET = new List<Ship>();
        }
        private void InitCast(DataGridView dgv)
        {
            dgv.RowCount = 10;
            dgv.ColumnCount = 10;
            dgv.ScrollBars = ScrollBars.None;
        }
        // получаем клиента и поток с формы подключения
        public void RecieveParameters(TcpClient client, NetworkStream stream, string ipServer)
        {
            this.client = client;
            this.stream = stream;
            this.ipServer = ipServer;
        }
        // сетаем корабли
        private void dgvYourSea_KeyDown(object sender, KeyEventArgs e)
        {
            //(чтобы не создавалось пустое пространство в таблице под строками)
            dgvYourSea.SuspendLayout(); // приостановка отрисовки таблицы
            if (e.KeyCode == Keys.Enter)
            {
                foreach(DataGridViewCell cell in dgvYourSea.SelectedCells)
                {
                    // заносим координаты чтобы потом передать обстановку на сервер
                    Point p = new Point(cell.ColumnIndex, cell.RowIndex);
                    selectedCellsCoordlist.Add(p);
                }
                if (EnterShip())
                    foreach (DataGridViewCell cell in dgvYourSea.SelectedCells)
                        cell.Style.BackColor = Color.BlueViolet;
            }
            dgvYourSea.ResumeLayout(); // возвращение 
        }
        /// <summary>
        /// Я ГОТОВ! (отправить сет Кораблей на Сервер)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void butStart_Click(object sender, EventArgs e)
        {
            // контрольная сумма на 20 закрашенных клеток
            int controlsum = 0;
            for (int col = 0; col < 10; col++)
                for (int row = 0; row < 10; row++)
                    if (dgvYourSea[col, row].Style.BackColor == Color.BlueViolet) controlsum++;
            if (controlsum != 20)
            {
                MessageBox.Show("Итого на карте должно быть ровно 20 клеток!");
                resetShip();
                return;
            }

            MessageBox.Show("Корабли отправляются в плавание на сервер!");

            // Конвертация сета кораблей на сервер  (ПОПЫТКА 3)
            string json = JsonConvert.SerializeObject(shipSET);
            byte[] message;
            
            message = Encoding.UTF8.GetBytes("rasst" + json); //метка+корабли

            client = new TcpClient(ipServer, 8888);
            stream = client.GetStream();
            try
            {
                //отправляем ответ с Клиента
                stream.Write(message, 0, message.Length);
                //собираем ответ с Сервака
                byte[] responseBytes = new byte[1024];
                int bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
                string response = Encoding.ASCII.GetString(responseBytes, 0, bytesRead);
                if(!response.Contains("OK")) resetAllShips();
                //если все хорошо, то сервер вернет "OK"

                if(!(response == "OK"))
                {
                    message = Encoding.UTF8.GetBytes("ImReady!");
                    stream = client.GetStream();
                    stream.Write(message, 0, message.Length);
                    responseBytes = new byte[1024];
                    bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
                    response = Encoding.UTF8.GetString(responseBytes, 0, bytesRead);
                    //
                    if (response.Contains("NotYet"))
                    {
                        MessageBox.Show(response + "\nПодождите второго игрока и нажмите ещё раз...");
                        return;
                    }
                    if (response.Contains("GameStarted"))
                    {
                        MessageBox.Show("Вы начали игру с противником!");
                        GameTransformation(response);
                    }
                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message + ex.ErrorCode);
            }

            return;
           
        }
        private void resetAllShips()
        {
            shipSET.Clear();
            foreach (DataGridViewRow row in dgvYourSea.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Style.BackColor = Color.White;
        }
        private void resetShip()
        {
            foreach (DataGridViewCell cell in dgvYourSea.SelectedCells)
            {
                cell.Style.BackColor = Color.White;
                // заносим координаты чтобы потом передать обстановку на сервер
                Point p = new Point(cell.ColumnIndex, cell.RowIndex);
                selectedCellsCoordlist.Remove(p);
            }
        }
        // TODO: реализовать проверку на квадрат
        private bool CheckSquare()
        {
            bool IsSquare = false;
            return IsSquare;
        }

        /// <summary>
        /// ДАБАВИТЬ ВЫДЕЛЕННЫЙ КОРАБЛЬ В СЕТ
        /// </summary>
        private bool EnterShip()
        {
            // TODO: обработать ситуацию "4ка-квадрат"
            if (selectedCellsCoordlist.Count == 4 && CheckSquare())
            {
                resetShip();
                MessageBox.Show("Невозможный корабль!");
                return false;
            }

            Ship ship = new Ship(0, new List<SeaCell>(), Ship.vectoring.horizontal);
            foreach (Point p in selectedCellsCoordlist)
            {
                // узнаем истинную природу корабля в контексте разворота на 2й клетке
                if (ship.type == 1 && (p.Y - 1 == ship.cellList[0].coordinate.Y || p.Y + 1 == ship.cellList[0].coordinate.Y))
                    ship.vec = Ship.vectoring.vertical;
                // меняем местами игреки с иксами из-за датагрида
                Point currentP = new Point(p.X, p.Y);
                // Если оба правила выполняются (мощность корабля и смежность клеток)
                if (Rule2(currentP) && ship_not_contact(ship, currentP))
                {
                    SeaCell cell = new SeaCell(currentP, SeaCell.state.filled_ship);
                    ship.cellList.Add(cell);
                    ship.type++;
                }
                else
                {
                    MessageBox.Show("Вы нарушили правила расстановки кораблей!");
                    // стираем выделенные клетки
                    resetShip();
                    return false;
                }

            }
            if (ship.type == 0 || ship.type > 4) //Rule1: ТИП КОРАБЛЯ
            {
                MessageBox.Show("Вы не выставили корабль или сделали его больше чем на 4 клетки!");
                resetShip();
                return false;
            }
            else
            {
                
                shipSET.Add(ship);
                selectedCellsCoordlist.Clear(); // обновляем буфер
                
                // DeBug
                MessageBox.Show("Корабль добавлен в буфер!");
                return true;
            }
        }

        /// <summary>
        /// УГЛЫ
        /// </summary>
        /// <param name="cell">координаты клеток</param>
        /// <returns>true если по углам нет закрашенных клеток</returns>
        private bool Rule2(Point cell)
        {
            bool existOnTopLeft, existOnBottomLeft, existOnTopRight, existOnBottomRight;
            if (cell.X == 0) // левая граница
            {
                if (cell.Y == 0) // верхняя граница
                {
                    existOnBottomRight = dgvYourSea[cell.X + 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
                    return !existOnBottomRight;
                }
                if (cell.Y == 9) // нижняя граница
                {
                    existOnTopRight = dgvYourSea[cell.X + 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
                    return !existOnTopRight;
                }
                existOnTopRight = dgvYourSea[cell.X + 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
                existOnBottomRight = dgvYourSea[cell.X + 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
                return !(existOnBottomRight || existOnTopRight);
            }
            if (cell.X == 9) // правая граница
            {
                if (cell.Y == 0) // верхняя граница
                {
                    existOnBottomLeft = dgvYourSea[cell.X - 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
                    return !existOnBottomLeft;
                }
                if (cell.Y == 9) // нижняя граница
                {
                    existOnTopLeft = dgvYourSea[cell.X - 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
                    return !existOnTopLeft;
                }
                existOnTopLeft = dgvYourSea[cell.X - 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
                existOnBottomLeft = dgvYourSea[cell.X - 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
                return !(existOnTopLeft || existOnBottomLeft);
            }
            if (cell.Y == 0) // верхняя граница
            {
                if (cell.X == 0) // верхний левый угол
                {
                    existOnBottomLeft = dgvYourSea[cell.X - 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
                    return !existOnBottomLeft;
                }
                if (cell.X == 9) // верхний правый угол
                {
                    existOnBottomRight = dgvYourSea[cell.X + 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
                    return !existOnBottomRight;
                }
                existOnBottomLeft = dgvYourSea[cell.X - 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
                existOnBottomRight = dgvYourSea[cell.X + 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
                return !(existOnBottomLeft || existOnBottomRight);
            }
            if (cell.Y == 9) // нижняя граница
            {
                if (cell.X == 0) // нижний левый угол
                {
                    existOnTopLeft = dgvYourSea[cell.X - 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
                    return !existOnTopLeft;
                }
                if (cell.X == 9) // нижний правый угол
                {
                    existOnTopRight = dgvYourSea[cell.X + 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
                    return !existOnTopRight;
                }
                existOnTopLeft = dgvYourSea[cell.X - 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
                existOnTopRight = dgvYourSea[cell.X + 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
                return !(existOnTopLeft || existOnTopRight);
            }
            // Если по углам от клетки есть другие выбранные клетки
            existOnTopLeft = dgvYourSea[cell.X - 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
            existOnBottomLeft = dgvYourSea[cell.X - 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
            existOnTopRight = dgvYourSea[cell.X + 1, cell.Y - 1].Style.BackColor == Color.BlueViolet;
            existOnBottomRight = dgvYourSea[cell.X + 1, cell.Y + 1].Style.BackColor == Color.BlueViolet;
            
            return !(existOnBottomLeft || existOnBottomRight || existOnTopLeft || existOnTopRight); 
        }
        /// <summary>
        /// Проверяет нет ли примыкающих смежных кораблей к выделенному
        /// </summary>
        /// <returns>true если рядом нет конфликтующих кораблей</returns>
        private bool ship_not_contact(Ship currentShip, Point currentP)
        {
            bool right, left, top, down;
            if(currentShip.type == 0)
            {
                
                // Левая стена
                if (currentP.X == 0)
                {
                    right = dgvYourSea[currentP.X + 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                    // Верхняя стена
                    if (currentP.Y == 0)
                    {
                        down = dgvYourSea[currentP.X, currentP.Y + 1].Style.BackColor == Color.BlueViolet;
                        if (right || down) return false;
                        else return true;
                    }
                        
                    // Нижняя стена
                    if (currentP.Y == 9)
                    {
                        top = dgvYourSea[currentP.X, currentP.Y - 1].Style.BackColor == Color.BlueViolet;
                        if (right || top) return false;
                        else return true;
                    }

                    // дефолт
                    top = dgvYourSea[currentP.X, currentP.Y - 1].Style.BackColor == Color.BlueViolet;
                    down = dgvYourSea[currentP.X, currentP.Y + 1].Style.BackColor == Color.BlueViolet;
                    if (right || top || down)
                        return false;
                    else return true;
                }
                // Правая стена
                if (currentP.X == 9)
                {
                    // Верхняя стена
                    if (currentP.Y == 0)
                    {
                        left = dgvYourSea[currentP.X - 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                        down = dgvYourSea[currentP.X, currentP.Y + 1].Style.BackColor == Color.BlueViolet;
                        if (left || down) return false;
                        else return true;
                    }
                        
                    // Нижняя стена
                    if (currentP.Y == 9)
                    {
                        left = dgvYourSea[currentP.X - 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                        top = dgvYourSea[currentP.X, currentP.Y - 1].Style.BackColor == Color.BlueViolet;
                        if (left || top) return false;
                        else return true;
                    }

                    // дефолт
                    left = dgvYourSea[currentP.X - 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                    top = dgvYourSea[currentP.X, currentP.Y - 1].Style.BackColor == Color.BlueViolet;
                    down = dgvYourSea[currentP.X, currentP.Y + 1].Style.BackColor == Color.BlueViolet;
                    if (left || top || down)
                        return false;
                    else return true;
                }
                // Верхняя стена
                if (currentP.Y == 0)
                {
                    left = dgvYourSea[currentP.X - 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                    down = dgvYourSea[currentP.X, currentP.Y + 1].Style.BackColor == Color.BlueViolet;
                    if (left || down) return false;
                    else return true;
                }
                // Нижняя стена
                if (currentP.Y == 9)
                {
                    left = dgvYourSea[currentP.X - 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                    top = dgvYourSea[currentP.X, currentP.Y - 1].Style.BackColor == Color.BlueViolet;
                    if (left || top) return false;
                    else return true;
                }
                left = dgvYourSea[currentP.X - 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                right = dgvYourSea[currentP.X + 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                top = dgvYourSea[currentP.X, currentP.Y - 1].Style.BackColor == Color.BlueViolet;
                down = dgvYourSea[currentP.X, currentP.Y + 1].Style.BackColor == Color.BlueViolet;
                if (left || right || top || down)
                {
                    return false;
                }
                else return true; 
            }
            else
            {
                if(currentShip.vec == Ship.vectoring.vertical)
                {
                    // левая стена
                    if (currentP.X == 0)
                    {
                        right = dgvYourSea[currentP.X + 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                        return !right;
                    }
                    // правая стена
                    if (currentP.X == 9)
                    {
                        left = dgvYourSea[currentP.X - 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                        return !left;
                    }
                    // дефолт
                    left = dgvYourSea[currentP.X - 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                    right = dgvYourSea[currentP.X + 1, currentP.Y].Style.BackColor == Color.BlueViolet;
                    if (left || right)
                        return false;
                    else return true;
                }
                else
                {
                    // верхняя стена
                    if (currentP.Y == 0)
                    {
                        down = dgvYourSea[currentP.X, currentP.Y + 1].Style.BackColor == Color.BlueViolet;
                        return !down;
                    }
                    // нижняя стена
                    if (currentP.Y == 9)
                    {
                        top = dgvYourSea[currentP.X, currentP.Y - 1].Style.BackColor == Color.BlueViolet;
                        return !top;
                    }
                    // дефолт
                    top = dgvYourSea[currentP.X, currentP.Y - 1].Style.BackColor == Color.BlueViolet;
                    down = dgvYourSea[currentP.X, currentP.Y + 1].Style.BackColor == Color.BlueViolet;
                    if (top || down)
                        return false;
                    else return true;
                }
            }
        }
        /// <summary>
        /// Преобразует Клиента, открывает чужое поле и закрывает для изменения свое
        /// </summary>
        private void GameTransformation(string whoseTurn)
        {
            if (whoseTurn == "" || whoseTurn == null || !whoseTurn.Contains("opponent"))
            { MessageBox.Show("Второй игрок ещё не расставил или не подключился. Нажмите эту кнопку позже."); return; }
            
            InitCast(dgvOpponentSea);
            if (whoseTurn.Contains("you")) 
            {
                MessageBox.Show("Ваш ход."); 
                return; 
            } 
            else MessageBox.Show("Сейчас ход противника.");
        }
    }
}
