using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Field
    {
        public List<List<SeaCell>> cells;
        public List<SeaCell> filledcells;
        public List<Ship> ships;

        public Field()
        {
            cells = new List<List<SeaCell>>();
            

            for (int row = 0; row <10; row++)
            {
                cells.Add(new List<SeaCell>()); //создаем строку ячеек
                for (int icell = 0; icell < 10; icell++) //заполняем строку свободными ячейками
                {
                    cells[row].Add(new SeaCell(new System.Drawing.Point(row, icell), SeaCell.state.free));
                }
            }
                
        }
        /// <summary>
        /// Заполняет объект текущего поля клетками на соответствующих координатах
        /// </summary>
        /// <param name="coordList"></param>
        public void fill(List<Ship> ships)
        {
            filledcells = new List<SeaCell>();
            foreach (Ship sh in ships)
            {
                filledcells.Union(sh.cellList);                
            }
            updateCells();
        } 


        private bool IsFill(int col, int row)
        {
            bool yes = false;
            foreach (SeaCell cell in filledcells)
            {
                if (cell.coordinate.Y == row && cell.coordinate.X == col)
                {
                    yes = true;
                    return yes;
                }
            }
            return yes;
        }
        private void mapwrite()
        {
            bool yes = false;
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (IsFill(col, row)) { Console.Write("■ "); } else { Console.Write("_ "); }
                }
                Console.WriteLine();
            }
        }

        //public void newscanmap()
        //{
        //    for(int row = 0; row<10; row++)
        //    {
        //        for(int col = 0; col<10; col++)
        //        {
        //            // Если текущая клетка существует в списке заполненных клеток
        //            if (filledcells.Exists(cell => cell.coordinate.Equals(cells[row][col].coordinate)) && filledcells.Exists(cell => cell.current_state.Equals(cells[row][col].current_state)))
        //            {
        //                // Создаем корабль
        //                //Ship ship = new Ship(1, new List<SeaCell>().Add(cells[row][col]), Ship.vectoring.horizontal);
        //            }

        //        }
        //    }
        //}

        


        /// <summary> 
        /// обновляем поле, заносим инфу о заполненных клетках
        /// </summary>
        public void updateCells()
        {
            foreach (List<SeaCell> row in cells)
                foreach (SeaCell c in row)
                    if (filledcells.Contains(c)) { c.current_state = SeaCell.state.filled_ship; };
        }
        public void shipsetStatWrite()
        {
            Console.WriteLine("Статистика о кораблях: ");
            int x4count = ships.Count(sh4 => sh4.type == 4);
            int x3count = ships.Count(sh3 => sh3.type == 3);
            int x2count = ships.Count(sh2 => sh2.type == 2);
            int x1count = ships.Count(sh1 => sh1.type == 1);

            Console.WriteLine($"Кораблей четверок: {x4count}");
            Console.WriteLine($"Кораблей троек: {x3count}");
            Console.WriteLine($"Кораблей двоек: {x2count}");
            Console.WriteLine($"Кораблей однерок: {x1count}");
        }
        /// <summary>
        /// Считаем соотношение кораблей и возвращаем в массив
        /// </summary>
        /// <returns>0 - кол-во одноклеточных кораблей и далее по аналогии</returns>
        public int[] shipsetStatReturn()
        {
            int x4count = ships.Count(sh4 => sh4.type == 4);
            int x3count = ships.Count(sh3 => sh3.type == 3);
            int x2count = ships.Count(sh2 => sh2.type == 2);
            int x1count = ships.Count(sh1 => sh1.type == 1);
            return new int[4] { x1count, x2count, x3count, x4count };
        }
    }
}
