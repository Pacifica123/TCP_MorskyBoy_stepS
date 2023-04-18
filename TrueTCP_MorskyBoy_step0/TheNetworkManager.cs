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

        byte[] buffer;
        string message;

        public TheNetworkManager(IPAddress serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
           // this.client = client;
           // this.stream = stream;
            listener = new TcpListener(IPAddress.Any, 8888); // собирает всех подряд клиентов
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
            Console.WriteLine("Сервер получил старт на порту 8888");
            //Начинаем бесконечное прослушивание
            while (true)
            {
                // TODO:
                /*
                 * if (оба игрока готовы) { Game.Start(); для начала самой игры; RecieveData(код перехода режимы игры на Клиенте); 
                 */
                TcpClient client = listener.AcceptTcpClient(); 
                Console.WriteLine("Зафиксирована активность клиента: {0}", client.Client.RemoteEndPoint);
                if (game.player1 == null) 
                { 
                    game.player1 = new Player(); 
                    game.player1.id = client.Client.RemoteEndPoint; 
                } 
                else if (game.player2 == null && client.Client.RemoteEndPoint != game.player1.id) 
                {
                    game.player2 = new Player();
                    game.player2.id = client.Client.RemoteEndPoint;
                }
                

                // создаем новый поток на каждого клиента для производительности и безопасности
                //Thread clientThread = new Thread(() => HandleClient(client));
                //clientThread.Start();

                HandleClient(client, gmForProcessGame);

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
            buffer = new byte[256];
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
            //(определяем куда в какой класс запихать функцию)
            if (message.Contains("rasst")) return GameManager.Rasstanovka(message);


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
