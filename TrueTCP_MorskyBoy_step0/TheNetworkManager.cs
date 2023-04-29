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
        string GlobalIP;
        int serverPort;

        TcpClient client;
        NetworkStream stream;
        TcpListener listener;

        static byte[] buffer;
        string message;

        static Game bufferGame;

        public TheNetworkManager(IPAddress serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
           // this.client = client;
           // this.stream = stream;
            listener = new TcpListener(IPAddress.Any, serverPort); // собирает всех подряд клиентов
        }

        public void ConnectToServer()
        {

        }
        public void DisconnectToServer() 
        {
            
        }
        public void Start(Game game, GameManager gmForProcessGame)
        {
            listener.Start();
            bufferGame = game;
            Console.WriteLine("Сервер получил старт на порту 8888");
            //Начинаем бесконечное прослушивание
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient(); 
                Console.WriteLine("Зафиксирована активность клиента: {0}", client.Client.RemoteEndPoint);
                if (bufferGame.player1 == null) 
                {
                    Console.WriteLine("Присоединился первый игрок");
                    bufferGame.player1 = new Player();
                    bufferGame.player1.id = client.Client.RemoteEndPoint; 
                    bufferGame.currentPlayer = bufferGame.player1;
                } 
                else if (bufferGame.player2 == null && !bufferGame.player1.id.ToString().Contains(client.Client.RemoteEndPoint.ToString().Split(':')[0])) 
                {
                    Console.WriteLine("Присоединился второй игрок");
                    bufferGame.player2 = new Player();
                    bufferGame.player2.id = client.Client.RemoteEndPoint;
                    bufferGame.currentPlayer = bufferGame.player2;
                }
                
                if(bufferGame.player1.id.ToString().Contains(client.Client.RemoteEndPoint.ToString().Split(':')[0]))
                {
                    bufferGame.currentPlayer = bufferGame.player1;
                }
                else bufferGame.currentPlayer = bufferGame.player2;

                // TODO: (если останется время)
                // создаем новый поток на каждого клиента для производительности и безопасности
                //Thread clientThread = new Thread(() => HandleClient(client));
                //clientThread.Start();

                HandleClient(client, gmForProcessGame);

                //TODO: if(Game.Started) {отправить player1 и player2 сообщение о начале игры}

                //DeBug     |      |  
                if (bufferGame.player1 != null && bufferGame.player2 != null)
                {
                    if (bufferGame.player1.status && bufferGame.player2.status)
                    {
                        
                        bufferGame.GameStarted = true;
                        game.currentPlayer = game.player1;
                        //TODO: передать обоим игрокам на Клиент что они оба готовы
                        TcpClient player1 = new TcpClient(game.player1.id.ToString().Split(':')[0], serverPort);
                        SendData("your_turn", player1.GetStream());

                        TcpClient player2 = new TcpClient(game.player2.id.ToString().Split(':')[0], serverPort);
                        SendData("opponent_turn", player2.GetStream());
                        client.Close();
                    }
                }
            }
        }

        #region ПРИВАТНАЯ СЕКЦИЯ 
        // работает автономно
        
        /// <summary>
        /// ОТПРАВИТЕЛЬ
        /// </summary>
        /// <param name="answerServer">ответ Сервера</param>
        /// <param name="stream">поток для передачи Клиенту</param>
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
        /// <summary>
        /// ПОЛУЧАТЕЛЬ
        /// </summary>
        /// <param name="stream">поток чтения данных</param>
        private void ReceiveData(NetworkStream stream)
        {
            // метод принимает данные от другого игрока
            buffer = new byte[2048];
            int bytesRead;

            //while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            //{
            //    message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //    Console.WriteLine("Сервер получил сообщение: {0}", message);
            //}
            bytesRead = stream.Read(buffer, 0, buffer.Length);
            message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
           // if (message.Contains("rasst")) { };
            Console.WriteLine("Сервер получил сообщение: {0}", message);
        }

        /// <summary>
        /// ОБРАБОТЧИК СООБЩЕНИЙ-КОМАНД
        /// (в зависимости от команды выбирает что делать)
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Возвращает ОК по дефолту. Ответ сервера если предусмотрен.</returns>
        private static string ProcessMessage(string message)
        {
            // TODO: здесь будем реализовывать узел логики игры...
            if (message == "good") return "conected"; //проверка работы сервера
            // Массив кораблей для расстановки:
            if (message.Contains("rasst")) return GameManager.Rasstanovka(message.Replace("rasst", ""), bufferGame);


            return "OK";
        }
        /// <summary>
        /// ОБРАБОТЧИК ЗАПРОСОВ КЛИЕНТА
        /// </summary>
        /// <param name="client">Точка входа игры с Клиента</param>
        private void HandleClient(TcpClient client, GameManager gameManager) //точка входа игры с Клиента
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
