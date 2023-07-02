using Server2.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using ProtoBuf;

namespace Server2
{
    public class NetworkManager
    {
        private List<Game> games;
        private int GAME_ID_COUNTER = 0;

        public NetworkManager()
        {
            games = new List<Game>();
        }
        /// <summary>
        /// Весь общий процесс работы сервера
        /// </summary>
        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            while (true)
            {
                // Принимаем нового клиента или запрос
                TcpClient client = listener.AcceptTcpClient();
                // Проверяем, является ли клиент уже участником какой-либо игры
                if (IsClientAlreadyInGame(client))
                {
                    // Если клиент уже числится в игре, игнорируем его подключение
                    Console.WriteLine("Клиент уже участвует в игре. Подключение игнорируется.");
                    Task.Run(() => ProcessClient(client));
                    continue;
                }
                // Проверяем все возможные случаи подключения нового клиента
                if (games.Count == 0)
                {
                    // Если список игр пустой, создаем новую игру и добавляем клиента в нее
                    Game newGame = new Game(GAME_ID_COUNTER++);
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
                        Game newGame = new Game(GAME_ID_COUNTER++);
                        games.Add(newGame);
                        AddPlayerToGame(client, newGame);
                    }
                }
                Task.Run(() => ProcessClient(client));
                
            }
        }
        /// <summary>
        /// Главный механизм обработки запросов клиентов и ответа на них
        /// </summary>
        /// <param name="client"></param>
        private void ProcessClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[4000];
            int bytesRead;

            while (client.Connected && (bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string response = ProcessMessage(message, client);

                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);
            }

            client.Close();
        }
        /// <summary>
        /// ядро механизма обработки запросов клиентов
        /// </summary>
        /// <param name="message">полученное сообщение</param>
        /// <param name="client"></param>
        /// <returns></returns>
        private string ProcessMessage(string message, TcpClient client)
        {
            // Обработка полученного сообщения и возвращение ответа
            string id = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            switch (message)
            {
                // подключени
                case "test":
                    return "Good!";
                // расстановка
                case var placementMessage when placementMessage.StartsWith("placement:"):
                    return placementShips(placementMessage.Substring("placement:".Length), client);
                // периодический спрос клиента после расстановки
                case "YouReady?":
                    return CheckSecondPlayer(FindPlayerById(id));
                // ход атакующего
                case var attackMessage when attackMessage.StartsWith("Attack:"):
                    string coordinates = attackMessage.Substring("Attack:".Length);
                    ProcessAttack(coordinates, client);
                    //return ("AttackResult"+ProcessAttack(coordinates, client));
                    return "";
                // периодический спрос атакуемого
                //case "OpponentAlreadyAtacked?":
                //    Player thisPlayer = FindPlayerById(id);
                //    return ProcessAttackForOpponent(FindGameById(thisPlayer.GameId));
                case "disconnect":
                    RemovePlayer(id);
                    return ""; //игроку уже ничто не нужно после отключения
                //case "get_state":
                //    return (GetStateBin_String(id));
                case "get_last":
                    return ("LastTurn:" + JsonConvert.SerializeObject(FindGameById(FindPlayerById(id).GameId).LastTurn));
                // непонятная дичь
                default:
                    return "Default";
            }
        }
        //private string GetStateBin_String(string id)
        //{
        //    Game thisGame = FindGameById(FindPlayerById(id).GameId);
        //    Console.WriteLine($"Игрок {id} запросил текущее состояние игры");

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        Serializer.Serialize(stream, thisGame);
        //        byte[] serializedData = stream.ToArray();
        //        string stateString = "STATE" + Convert.ToBase64String(serializedData);
        //        return stateString;
        //    }
        //}
        //private string GetStateJSON(string id)
        //{
        //    Game thisGame = FindGameById(FindPlayerById(id).GameId);
        //    Console.WriteLine($"Игрок {id} запросил текущее состояние игры");
        //    return JsonConvert.SerializeObject(thisGame);
        //}

        private void RemovePlayer(string id)
        {
            Player thisPlayer = FindPlayerById(id);
            Game thisGame = FindGameById(thisPlayer.GameId);
            thisGame.Players.Remove(thisPlayer);
            
        }

        /// <summary>
        /// Ответ на каждый 10-секундый вопрос клиента о том сходил ли оппонент
        /// </summary>
        /// <returns>информацию о том, попал ли оппонент и куда в формате "opponent_*,X,Y"</returns>
        /// <exception cref="NotImplementedException"></exception>
        //private string ProcessAttackForOpponent(Game game)
        //{
        //    string answer = "OpponentAttackResult:";
        //    if (game.LastTurn != null || game.LastTurn.resultForNextPlayer == null)
        //    {
        //        answer += game.LastTurn.resultForNextPlayer; //tokens[0]
        //    }
        //    else
        //    {
        //        answer += "NotAlready"; //tokens[0]
        //    } 
            
        //    answer += "," + game.LastTurn.X.ToString(); //tokens[1]
        //    answer += "," + game.LastTurn.Y.ToString(); //tokens[2]
        //    return answer;
        //}

        /// <summary>
        /// Расстановка кораблей для игрока
        /// </summary>
        /// <param name="placementJSON">сериализованная строка - список кораблей</param>
        /// <param name="client"></param>
        /// <returns>тоже что и CheckSecondPlayer</returns>
        private string placementShips(string placementJSON, TcpClient client)
        {
            
            Console.WriteLine(new string('-', 10));
            Console.WriteLine("Десериализация следующей расстановки:");
            Console.WriteLine(placementJSON);
            Console.WriteLine(new string('-', 10));

            // Десериализация списка кораблей
            List<Ship> ships = JsonConvert.DeserializeObject<List<Ship>>(placementJSON);
            // Поиск соответствующего клиента по идентификатору игрока
            Player player = FindPlayerById(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

            // Если клиент найден, присваиваем корабли игроку
            if (player != null)
            {
                
                player.PlayerSea.Ships = ships;

                // Обновление списка клеток SeaCells в связи с новыми кораблями
                List<SeaCell> seaCells = player.PlayerSea.UpdateSeaCells(ships);
                player.PlayerSea.SeaCells = seaCells;

                player.Status = true;

                Console.WriteLine(new string('-', 10));
                Console.WriteLine($"Текущая расстановка у игрока {player.PlayerId} игры {player.GameId}");
                player.PlayerSea.PrintSeaMap();
                Console.WriteLine(new string('-', 10));

                return CheckSecondPlayer(player);
            }
            else
            {
                return "Player not found";
            }
        }
        /// <summary>
        /// Проверка на второго игрока и его гоовность для первого
        /// </summary>
        /// <param name="player"></param>
        /// <returns>результат проверки как ответ сервера клиенту</returns>
        private string CheckSecondPlayer(Player player)
        {
            Game game = FindGameById(player.GameId);

            if (game == null)
            {
                return "GameNotFound";
            }

            Player secondPlayer = game.Players.FirstOrDefault(p => p != player);

            if (secondPlayer == null)
            {
                return "DontSecond";
            }

            if (!player.Status || !secondPlayer.Status)
            {
                return "NotReady";
            }

            game.GameStarted = true;
            game.CurrentPlayer = game.Players[0];

            return "GameStarted:"+game.CurrentPlayer.PlayerId; //чей ход
        }
        private Player FindPlayerById(string playerId)
        {
            foreach (var game in games)
            {
                foreach (var player in game.Players)
                {
                    if (player.PlayerId == playerId)
                    {
                        return player;
                    }
                }
            }

            return null; // Игрок с указанным идентификатором не найден
        }
        private Game FindGameById(int gameId)
        {
            return games.FirstOrDefault(g => g.GameId == gameId);
        }
        private bool IsClientAlreadyInGame(TcpClient client)
        {
            string clientId = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            foreach (Game game in games)
            {
                foreach (Player player in game.Players)
                {
                    if (player.PlayerId == clientId)
                    {
                        return true; // Клиент уже числится в игре
                    }
                }
            }

            return false; // Клиент не числится в игре
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
            // Получение IP-адреса клиента
            string ipAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();

            return ipAddress;
        }
        private void ProcessAttack(string coordinates, TcpClient client)
        {
            string id = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
            Player thisPlayer = FindPlayerById(id);
            Game thisGame = FindGameById(thisPlayer.GameId);
            // Проверка правила чередования хода
            //if (thisGame.CurrentPlayer != FindPlayerById(id)) 
            //{
            //    return "NotYourTurn"; // Возвращаем сообщение, что сейчас не ваш ход
            //}

            // Обработка атаки на сервер
            bool isHit = thisGame.CheckAttack(coordinates); // Проверяем атаку на попадание
            if (isHit)
            {
                thisGame.CurrentPlayer = thisPlayer; // Ход остается у текущего клиента
                //return ("you_shot,"+coordinates);
            }
            else
            {
                // Ход передается оппоненту
                thisGame.ChangePlayer();

                //return ("you_fail,"+coordinates);
            }
        }


    }
}
