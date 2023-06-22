using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    public class Game
    {
        public int GameId { get; set; }
        public List<Player> Players { get; set; }
        public Player CurrentPlayer { get; set; }
        public bool IsFilledGame { get; set; }
        public bool GameStarted { get; set; }
        public bool GameOver { get; set; }

        public Game()
        {
            Players = new List<Player>();
        }

        public void ChangePlayer()
        {
            if (CurrentPlayer == Players[0])
            {
                CurrentPlayer = Players[1];
            }
            else
            {
                CurrentPlayer = Players[0];
            }
        }

        public bool ShipCorrect()
        {
            int[] shipCounts = new int[4]; // 4x1, 3x2, 2x3, 1x4

            foreach (var player in Players)
            {
                foreach (var ship in player.PlayerSea.Ships)
                {
                    shipCounts[ship.Type - 1]++;
                }
            }

            return shipCounts[0] == 4 && shipCounts[1] == 6 && shipCounts[2] == 6 && shipCounts[3] == 4;
        }
    }
}
