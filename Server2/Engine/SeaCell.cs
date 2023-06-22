using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    public class SeaCell
    {
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
