using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    public class SeaCell
    {
        public SeaCell(int x, int y, CellState state)
        {
            X = x;
            Y = y;
            this.State = state;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public enum CellState
        {
            Free,
            OccupiedByShip,
            Attacked
        }
        public CellState State { get; set; }
    }
}
