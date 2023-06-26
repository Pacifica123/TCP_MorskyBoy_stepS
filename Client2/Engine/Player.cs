using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    public class Player
    {
        public string PlayerId { get; set; }
        public int GameId { get; set; }
        public bool Status { get; set; }
        public Sea PlayerSea { get; set; }

        public Player(string id, Game game)
        {
            PlayerId = id;
            GameId = game.GameId;
            Status = false;
            PlayerSea = new Sea();
        }
        [JsonConstructor]
        public Player(string id, int gameID)
        {
            PlayerId = id;
            GameId = gameID;
            Status = false;
            PlayerSea = new Sea();
        }
    }
}
