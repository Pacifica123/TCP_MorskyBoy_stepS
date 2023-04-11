using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.forms
{
    public partial class Main : Form
    {
        public List<Point> selectedCellsCoordlist;
        private TcpClient client;
        private NetworkStream stream;
        private string ipServer;

        public Main()
        {
            InitializeComponent();
            InitCast();
            selectedCellsCoordlist = new List<Point>();
        }
        private void InitCast()
        {
            dgvYourSea.RowCount = 10;
            dgvYourSea.ColumnCount = 10;
            normalizeSea();
        }
        private void normalizeSea()
        {
            int totalHieght = 0;
            dgvYourSea.ScrollBars = ScrollBars.None;
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
                    cell.Style.BackColor = Color.BlueViolet;
                    // заносим координаты чтобы потом передать обстановку на сервер
                    Point p = new Point(cell.RowIndex, cell.ColumnIndex);
                    selectedCellsCoordlist.Add(p);
                }
            }
            dgvYourSea.ResumeLayout(); // возвращение 
        }

        private void butStart_Click(object sender, EventArgs e)
        {
            string message = "";
            foreach (Point p in selectedCellsCoordlist)
            {
                message += p.ToString();
            }
            client = new TcpClient(ipServer, 8888);
            stream = client.GetStream();
            
            byte[] messageBytes = Encoding.ASCII.GetBytes("rasst" + message);
            try
            {
                //отправляем кодовое слово для проверки работы сервера
                stream.Write(messageBytes, 0, messageBytes.Length);
                //собираем ответ с Сервака
                byte[] responseBytes = new byte[1024];
                int bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
                string response = Encoding.ASCII.GetString(responseBytes, 0, bytesRead);
                //если все хорошо, то сервер вернет "conected"
                MessageBox.Show(response);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
