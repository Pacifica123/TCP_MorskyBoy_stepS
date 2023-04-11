using System;
using System.Collections.Generic;
using System.Linq;
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
        public void fill(List<System.Drawing.Point> coordList)
        {
            filledcells = new List<SeaCell>();
            foreach (System.Drawing.Point p in coordList)
            {
                SeaCell cell = new SeaCell(p, SeaCell.state.filled_ship); // заполняем поле занятыми кораблями клетками
                filledcells.Add(cell);
                updateCells(p);
            }
        } 
        /// <summary>
        /// сканирует сразу всю карту для полноценного расставления всех кораблей.
        /// </summary>
        public void scanmap()
        {
            
        }
        /// <summary> 
        /// обновляем поле, заносим инфу о заполненных клетках
        /// </summary>
        public void updateCells(System.Drawing.Point p)
        {
            foreach (List<SeaCell> row in cells)
                foreach (SeaCell c in row)
                    if (c.coordinate == p) { c.current_state = SeaCell.state.filled_ship; };
        }
    }
}
