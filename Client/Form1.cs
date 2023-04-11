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
using Client.forms;
using Microsoft.VisualBasic;

namespace Client
{
    public partial class Form1 : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string ipServer = txtServerAddress.Text;
                Regex ipv4pattern = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
                while (!ipv4pattern.IsMatch(ipServer))
                {
                    MessageBox.Show("Неправильный формат ввода IP!");
                    ipServer = Interaction.InputBox(ipServer, "Введите корректный IP cервера");
                }
                //в текстбоксе указываем адрес сервера
                client = new TcpClient(ipServer, 8888);
                stream = client.GetStream();
                txtLog.AppendText("Подключение к серверу прошло успешно\n");

                //Это заготовка для отправки сообщения
                //(здесь использовал для проверки подключения)
                //----------------------------------------------------------
                //if (client == null || !client.Connected)
                //{
                //    txtLog.AppendText("Not connected to server\n");
                //    return;
                //}

                string message = "good";
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);

                try
                {
                    //отправляем кодовое слово для проверки работы сервера
                    stream.Write(messageBytes, 0, messageBytes.Length);
                    //собираем ответ с Сервака
                    byte[] responseBytes = new byte[1024];
                    int bytesRead = stream.Read(responseBytes, 0, responseBytes.Length);
                    string response = Encoding.ASCII.GetString(responseBytes, 0, bytesRead);
                    //если все хорошо, то сервер вернет "conected"
                    txtLog.AppendText("Ответ сервера: " + response + "\n");
                }
                catch (Exception ex)
                {
                    txtLog.AppendText("Ошибка в получении/отправки сообщения: " + ex.Message + "\n");
                }
                //----------------------------------------------------------

                Main main = new Main();
                main.RecieveParameters(client, stream, ipServer);
                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                txtLog.AppendText("Ошибка подключения к серверу: " + ex.Message + "\n");
            }
        }

        // корректное завершение работы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null && client.Connected)
            {
                client.Close();
            }
        }
    }
}
