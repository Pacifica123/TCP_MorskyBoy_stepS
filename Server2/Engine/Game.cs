using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    [ProtoContract]
    public class Game
    {
        [ProtoMember(1)]
        public int GameId { get; set; }
        [ProtoMember(2)]
        public List<Player> Players { get; set; }
        [ProtoMember(3)]
        public Player CurrentPlayer { get; set; }
        [ProtoMember(4)]
        public bool IsFilledGame { get; set; }
        [ProtoMember(5)]
        public bool GameStarted { get; set; }
        [ProtoMember(6)]
        public bool GameOver { get; set; }
        [ProtoMember(7)]
        public Turn? LastTurn { get; set; }


   

        public Game(int id)
        {
            Players = new List<Player>();
            GameId = id;
            LastTurn = new Turn();
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
        /// <summary>
        /// Проверка на попадание и обновление последнего хода
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns>попал/не попал(или нельзя уже)</returns>
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

            //Turn lastTurn = new Turn
            //{
            //    AtackedPlayer = opponent,
            //    Atacker = CurrentPlayer,
            //    X = x,
            //    Y = y
            //};
            LastTurn.AtackerID = CurrentPlayer.PlayerId;
            LastTurn.AtackedPlayerID = opponent.PlayerId;
            LastTurn.X = x;
            LastTurn.Y = y;

            // Проверка, попал ли атакующий по кораблю
            if (targetCell.State == SeaCell.CellState.OccupiedByShip)
            {
                // Получение объекта Ship, находящегося на атакованной клетке
                Ship targetShip = opponent.PlayerSea.Ships.Find(ship => ship.ContainsCell(targetCell));

                // Нанесение повреждения кораблю
                targetShip.Damage();
                targetCell.State = SeaCell.CellState.Attacked;
                LastTurn.resultForNextPlayer = "shot";

                return true; // Атака успешна (попадание по кораблю)
            }
            // Обновление состояния целевой клетки
            targetCell.State = SeaCell.CellState.Attacked;
            LastTurn.resultForNextPlayer = "fail";
           // LastTurn = lastTurn;
            return false; // Атака неудачна (промах)
        }
    }
    //TODO: добавить в ЛастХод кол-во кораблей или другой индикатор окончания игры
    [ProtoContract]
    public class Turn
    {
        [ProtoMember(1)]
        public string AtackedPlayerID { get; set; }
        [ProtoMember(2)]
        public string AtackerID { get; set; }
        [ProtoMember(3)]
        public int X { get; set; }
        [ProtoMember(4)]
        public int Y { get; set; }
        [ProtoMember(5)]
        public string resultForNextPlayer { get; set; } //fail или shot - результат для текущего игрока
        public Turn()
        {
            //resultForNextPlayer = "NotAlredy";
        }
    }
}
