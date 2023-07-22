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

        /// <summary>
        /// Для заданной игры устанавливает флаг GameOver и в последний ход записывает IPID победитиля
        /// </summary>
        /// <param name="game"></param>
        /// <param name="win"></param>
        private void EndGame(Game game, Player win)
        {
            game.GameOver = true;
            game.LastTurn.resultForNextPlayer = "WIN:" + win.PlayerId;
        }

        /// <summary>
        /// Проверяет а также производит процесс завершения игры если true
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
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

        /// <summary>
        ///проверка все ли корабли разрушены на поле
        /// </summary>
        /// <param name="SeaOfShips"></param>
        /// <returns></returns>
        private bool CheckAllShipsDestructed(List<Ship> SeaOfShips)
        {
            if (SeaOfShips == null || SeaOfShips.Count == 0) return false;
            foreach (Ship ship in SeaOfShips)
            {
                if (ship.IsDestructed == false) return false;
            }
            return true;
        }
    }
}
