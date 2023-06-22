using Server2.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server2
{
    public class NetworkManager
    {
        private List<Game> games;
        private List<Player> waitingPlayers;

        public NetworkManager()
        {
            games = new List<Game>();
            waitingPlayers = new List<Player>();
        }
        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                // Принимаем нового клиента или запрос
                TcpClient client = listener.AcceptTcpClient();
                // Проверяем все возможные случаи подключения нового клиента
                if (games.Count == 0)
                {
                    // Если список игр пустой, создаем новую игру и добавляем клиента в нее
                    Game newGame = new Game();
                    games.Add(newGame);
                    AddPlayerToGame(client, newGame);
                }
                else
                {
                    bool clientAdded = false;

                    // Проверяем каждую игру
                    foreach (Game game in games)
                    {
                        if (game.Players.Count < 2)
                        {
                            // Если в игре есть место, добавляем клиента в игру
                            AddPlayerToGame(client, game);
                            clientAdded = true;
                            break;
                        }
                    }

                    if (!clientAdded)
                    {
                        // Если все игры заполнены, создаем новую игру и добавляем клиента в нее
                        Game newGame = new Game();
                        games.Add(newGame);
                        AddPlayerToGame(client, newGame);
                    }
                }
                Task.Run(() => ProcessClient(client));
            }
        }
        private void ProcessClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string response = ProcessMessage(message);

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }

            client.Close();
        }
        private string ProcessMessage(string message)
        {
            // Обработка полученного сообщения и возвращение ответа
            switch (message)
            {
                case "test":
                    return "Good!";

                default:
                    return "Default";
            }
        }
        private void AddPlayerToGame(TcpClient client, Game game)
        {
            // Создаем нового игрока с указанным ID и привязываем его к игре
            Player player = new Player(GeneratePlayerId(client), game);
            //game.AddPlayer(player);
            game.Players.Add(player);
            Console.WriteLine($"Присоединился игрок:{player.PlayerId} к игре: {player.GameId}");
            //client.AssignGame(game);
        }
        private string GeneratePlayerId(TcpClient client)
        {
            // Генерация нового GUID
            Guid playerIdGuid = Guid.NewGuid();
            string playerId = playerIdGuid.ToString();

            return playerId;
        }
    }
}
