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

        public void EndGame(Game game)
        {
            game.GameOver = true;
        }
        public void Rasstanaovka(Game game, string playerId, List<Ship> ships)
        {
            Player currentPlayer = game.Players.FirstOrDefault(p => p.PlayerId == playerId);

            if (currentPlayer != null)
            {
                currentPlayer.PlayerSea.Ships = ships;
            }
        }
        public string Turn(Game game, string attackingPlayerId, int x, int y)
        {
            Player attackingPlayer = game.Players.FirstOrDefault(p => p.PlayerId == attackingPlayerId);
            Player defendingPlayer = game.Players.FirstOrDefault(p => p.PlayerId != attackingPlayerId);

            if (attackingPlayer == null || defendingPlayer == null)
            {
                return "Ошибка: неверный игрок";
            }

            SeaCell targetCell = defendingPlayer.PlayerSea.SeaCells.FirstOrDefault(c => c.X == x && c.Y == y);

            if (targetCell == null)
            {
                return "Ошибка: неверные координаты";
            }

            if (targetCell.State == CellState.Attacked)
            {
                return "Ошибка: клетка уже атакована";
            }

            if (targetCell.State == CellState.OccupiedByShip)
            {
                targetCell.State = CellState.Attacked;
                Ship targetedShip = defendingPlayer.PlayerSea.Ships.FirstOrDefault(s => s.ShipCells.Contains(targetCell));

                if (targetedShip != null)
                {
                    targetedShip.Damage();
                    if (targetedShip.IsDestructed) defendingPlayer.PlayerSea.Ships.Remove(targetedShip); 
                }
            }

            return "Ход выполнен успешно";
        }
    }
}
