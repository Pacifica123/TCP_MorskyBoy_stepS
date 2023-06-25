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
        public Turn LastTurn { get; set; }



        public Game(int id)
        {
            Players = new List<Player>();
            GameId = id;

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

        public bool CheckAttack(string coordinates)
        {
            // Разделение координат атаки по запятой
            string[] coordinatesArray = coordinates.Split(',');

            // Проверка корректности координат атаки
            if (coordinatesArray.Length != 2)
            {
                return false; // Некорректный формат координат атаки
            }

            // Парсинг координат атаки
            if (!int.TryParse(coordinatesArray[0], out int x) || !int.TryParse(coordinatesArray[1], out int y))
            {
                return false; // Некорректные значения координат атаки
            }


            // Получение объекта SeaCell по заданным координатам
            Player opponent = CurrentPlayer == Players[0] ? Players[1] : Players[0];
            SeaCell targetCell = opponent.PlayerSea.SeaCells.Find(seaCell => seaCell.X == x && seaCell.Y == y);

            // Проверка состояния целевой клетки
            if (targetCell == null || targetCell.State == SeaCell.CellState.Attacked)
            {
                return false; // Атака невозможна (клетка уже атакована или некорректные координаты)
            }

            Turn lastTurn = new Turn
            {
                AtackedPlayer = opponent,
                Atacker = CurrentPlayer,
                X = x,
                Y = y
            };

            // Проверка, попал ли атакующий по кораблю
            if (targetCell.State == SeaCell.CellState.OccupiedByShip)
            {
                // Получение объекта Ship, находящегося на атакованной клетке
                Ship targetShip = opponent.PlayerSea.Ships.Find(ship => ship.ShipCells.Contains(targetCell));

                // Нанесение повреждения кораблю
                targetShip.Damage();
                targetCell.State = SeaCell.CellState.Attacked;
                lastTurn.resultForNextPlayer = "opponent_shot";

                return true; // Атака успешна (попадание по кораблю)
            }
            // Обновление состояния целевой клетки
            targetCell.State = SeaCell.CellState.Attacked;
            lastTurn.resultForNextPlayer = "opponent_fail";
            LastTurn = lastTurn;
            return false; // Атака неудачна (промах)
        }
    }

    public class Turn
    {
        public Player AtackedPlayer { get; set; }
        public Player Atacker { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string resultForNextPlayer { get; set; } //opponent_fail или opponent_shot - результат для текущего игрока
    }
}
