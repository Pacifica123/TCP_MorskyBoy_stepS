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
    /// <summary>
    ///     класс, который отвечает за управление сетевым соединением и передачу данных между игроками
    /// </summary>
    internal class TheNetworkManager
    {
        IPAddress serverIP;
        int serverPort;

        static TcpClient client;
        NetworkStream stream;
        TcpListener listener;

        static byte[] buffer;
        string message;

        static List<Game> bufferGames;
        static Dictionary<string, int> IP_ID;

        public TheNetworkManager(IPAddress serverIP, int serverPort)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
           // this.client = client;
           // this.stream = stream;
            listener = new TcpListener(IPAddress.Any, this.serverPort); // собирает всех подряд клиентов
        }

        public void ConnectToServer()
        {

        }
        public void DisconnectToServer() 
        {
            listener.Stop();
            if(bufferGames != null) bufferGames.Clear();
        }
        private static Game? SearchThisGame(string IP)
        {
            return bufferGames.FirstOrDefault(g => g.player1.id.ToString().Contains(IP) || (g.player2 != null && g.player2.id.ToString().Contains(IP)));
        }
        public void Start(Game game, GameManager gmForProcessGame)
        {
            listener.Start();
            bufferGames = new List<Game>();
            IP_ID = new Dictionary<string, int>();
            int idGame = 0;
            Console.WriteLine("Сервер получил старт на порту 8888");

            //Начинаем бесконечное прослушивание
            while (true)
            {
                client = listener.AcceptTcpClient(); 
                Console.WriteLine("Зафиксирована активность клиента: {0}", client.Client.RemoteEndPoint);
                // поиск возможной игры в которой игрок уже есть (вычисляем по IP, лол)
                Game? thisGame = SearchThisGame(client.Client.RemoteEndPoint.ToString().Split(':')[0]);

                // если игры не найдено, значит создаем новую, так подключившийся - совершенно новый игрок
                // (вроде бы bufferGames[bufferGames.Count - 1].IsFilledGame не обязательное )
                if (bufferGames.Count == 0 || bufferGames[bufferGames.Count - 1].IsFilledGame && thisGame == null)
                {
                    thisGame = new Game(idGame++);
                    bufferGames.Add(thisGame);
                }

                // если игра совершенно пустая - добавляем первого игрока как текущего игрока
                if (bufferGames[bufferGames.Count - 1].player1 == null) 
                {
                    Console.WriteLine($"Присоединился первый игрок к новой игре: {bufferGames[bufferGames.Count - 1].game_id}");
                    thisGame.player1 = new Player();
                    thisGame.player1.id = client.Client.RemoteEndPoint;
                    thisGame.player1.game_id = thisGame.game_id; 
                    thisGame.currentPlayer = thisGame.player1;
                } 
                // если клиент отличается от первого игрока, а второе свободно - занимаем его
                else if (thisGame.player2 == null && !thisGame.player1.id.ToString().Contains(client.Client.RemoteEndPoint.ToString().Split(':')[0])) 
                {
                    Console.WriteLine($"Присоединился второй игрок к игре: {thisGame.game_id}");
                    
                    thisGame.player2 = new Player();
                    thisGame.player2.id = client.Client.RemoteEndPoint;
                    thisGame.player2.game_id = thisGame.game_id;
                    thisGame.currentPlayer = thisGame.player2;
                }
                // здесь просто выбираем текущего игрока
                if(thisGame.player1.id.ToString().Contains(client.Client.RemoteEndPoint.ToString().Split(':')[0]))
                {
                    thisGame.currentPlayer = thisGame.player1;
                }
                else thisGame.currentPlayer = thisGame.player2;

                // TODO: (если останется время)
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
            Game currentGame = SearchThisGame(client.Client.RemoteEndPoint.ToString().Split(':')[0]);
            if (message == "good") return "conected"; //проверка работы сервера
            // Массив кораблей для расстановки:
            if (message.Contains("rasst")) return GameManager.Rasstanovka(message.Replace("rasst", ""), currentGame);
            // Переодическая проверка клиентом сервера по нажатию кнопки "Я готов!"
            // проблема: это может быть 2й игрок...
            if (message.Contains("ImReady!") && !currentGame.GameStarted)
                return "NotYet";
            if (message.Contains("ImReady!") && currentGame.GameStarted)
                return "GameStarted" + (client.Client.RemoteEndPoint.ToString().Split(':')[0] == currentGame.player1.id.ToString().Split(':')[0] ? "your_turn" : "opponent_turn");
            // TODO: обработка запроса о ходе:

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
