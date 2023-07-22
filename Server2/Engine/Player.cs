using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProtoBuf;

namespace Server2.Engine
{
    [ProtoContract]
    public class Player
    {
        [ProtoMember(1)]
        public string PlayerId { get; set; }
        [ProtoMember(2)]
        public int GameId { get; set; }
        [ProtoMember(3)]
        public bool Status { get; set; }
        [ProtoMember(4)]
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
