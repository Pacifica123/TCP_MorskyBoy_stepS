using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    public class Sea
    {
        public List<SeaCell> SeaCells { get; set; }
        public List<Ship> Ships { get; set; }

        public Sea()
        {
            SeaCells = new List<SeaCell>();
            Ships = new List<Ship>();
        }
        public List<SeaCell> UpdateSeaCells(List<Ship> ships)
        {
            List<SeaCell> updatedSeaCells = new List<SeaCell>();

            // Создаем пустое море с клетками состояния "Free"
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    updatedSeaCells.Add(new SeaCell { X = x, Y = y, State = SeaCell.CellState.Free });
                }
            }

            // Заполняем клетки, занятые кораблями, состоянием "OccupiedByShip"
            foreach (var ship in ships)
            {
                foreach (var cell in ship.ShipCells)
                {
                    updatedSeaCells.Find(seaCell => seaCell.X == cell.X && seaCell.Y == cell.Y).State = SeaCell.CellState.OccupiedByShip;
                }
            }

            return updatedSeaCells;
        }
        public void PrintSeaMap()
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    SeaCell cell = SeaCells.Find(seaCell => seaCell.X == x && seaCell.Y == y);

                    switch (cell.State)
                    {
                        case SeaCell.CellState.Free:
                            Console.Write("O ");
                            break;
                        case SeaCell.CellState.OccupiedByShip:
                            Console.Write("# ");
                            break;
                        case SeaCell.CellState.Attacked:
                            Console.Write("* ");
                            break;
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
