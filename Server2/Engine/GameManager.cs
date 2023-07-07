using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Server2.Engine.SeaCell;

namespace Server2.Engine
{
    public class GameManager
    {
        public void StartGame(Game game)
        {
            game.GameStarted = true;
        }

        public void EndGame(Game game, Player win)
        {
            game.GameOver = true;
            game.LastTurn.resultForNextPlayer = "WIN:" + win.PlayerId;
        }
        //public void Rasstanaovka(Game game, string playerId, List<Ship> ships)
        //{
        //    Player currentPlayer = game.Players.FirstOrDefault(p => p.PlayerId == playerId);

        //    if (currentPlayer != null)
        //    {
        //        currentPlayer.PlayerSea.Ships = ships;
        //    }
        //}
        public bool CheckGameOver(Game game)
        {
            if (game.Players.Count < 2) return false;
            
            if (CheckAllShipsDestructed(game.Players[0].PlayerSea.Ships) || CheckAllShipsDestructed(game.Players[1].PlayerSea.Ships))
            {
                EndGame(game, CheckAllShipsDestructed(game.Players[0].PlayerSea.Ships) ? game.Players[1] : game.Players[0]);
                return true;
            }
            return false;

        }
        public bool CheckAllShipsDestructed(List<Ship> SeaOfShips)
        {
            foreach (Ship ship in SeaOfShips)
            {
                if (ship.IsDestructed == false) return false;
            }
            return true;
        }
    }
}
