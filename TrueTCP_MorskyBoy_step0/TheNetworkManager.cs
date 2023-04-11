using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    //класс, который отвечает за управление сетевым соединением и передачу данных между игроками
    internal class TheNetworkManager
    {
        IPAddress serverIP;
        int serverPort;

        TcpClient client;
        NetworkStream stream;
        TcpListener listener;

        static GameManager gameManager;
        static Game game;

        byte[] buffer;
        string message;

        public TheNetworkManager(IPAddress serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
           // this.client = client;
           // this.stream = stream;
            listener = new TcpListener(IPAddress.Any, 8888); // собирает всех подряд клиентов
            gameManager = new GameManager();
            game = new Game();
        }

        public void ConnectToServer()
        {

        }
        public void DisconnectToServer() 
        {
            
        }
        public void Start()
        {
            listener.Start();
            Console.WriteLine("Сервер получил старт на порту 8888");
            //Начинаем бесконечное прослушивание
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient(); 
                Console.WriteLine("Зафиксирована активность клиента: {0}", client.Client.RemoteEndPoint);
                if (game.player1.id == null) { game.player1.id = client.Client.RemoteEndPoint; } 
                if (game.player2.id == null && game.player1.id != null && client.Client.RemoteEndPoint != game.player1.id) 
                {
                    game.player2.id = client.Client.RemoteEndPoint;
                }
                

                // создаем новый поток на каждого клиента для производительности и безопасности
                //Thread clientThread = new Thread(() => HandleClient(client));
                //clientThread.Start();

                HandleClient(client);

            }
        }

        #region ПРИВАТНАЯ СЕКЦИЯ 
        // работает автономно

        // ОТПРАВИТЕЛЬ 
        private void SendData(string answerServer, NetworkStream stream)
        {
            // метод отправляет другому игроку данные о ходе текущего
            //int bytesRead;

            //while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            //{
            //    byte[] answerServerBytes = Encoding.ASCII.GetBytes(answerServer);
            //    stream.Write(answerServerBytes, 0, answerServerBytes.Length);
            //}
            //bytesRead = stream.Read(buffer, 0, buffer.Length);
            byte[] answerServerBytes = Encoding.ASCII.GetBytes(answerServer);
            stream.Write(answerServerBytes, 0, answerServerBytes.Length);
        }
        // ПОЛУЧАТЕЛЬ
        private void ReceiveData(NetworkStream stream)
        {
            // метод принимает данные от другого игрока
            buffer = new byte[1024];
            int bytesRead;

            //while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            //{
            //    message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //    Console.WriteLine("Сервер получил сообщение: {0}", message);
            //}
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Сервер получил сообщение: {0}", message);
        }
        // ОБРАБОТЧИК СООБЩЕНИЙ
        private static string ProcessMessage(string message)
        {
            // TODO: здесь будем реализовывать логику игры...

            if (message == "good") return "conected"; //проверка работы сервера
            //(определяем куда в какой класс запихать функцию)
            if (message.Contains("rasst")) return gameManager.Rasstanovka(message);


            return "OK";
        }
        // ОБРАБОТЧИК ЗАПРОСОВ КЛИЕНТА
        private void HandleClient(TcpClient client) //точка входа игры с Клиента
        {
            NetworkStream stream = client.GetStream(); 
            ReceiveData(stream); //заносит в message запрос от Клиента
            SendData(ProcessMessage(message), stream); //Сервер отвечает используя message уже как буфер

            // не забываем закрывать клиента по обработке запроса
            client.Close();

        }
        #endregion
    }
}
