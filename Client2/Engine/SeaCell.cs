using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    [ProtoContract]
    public class SeaCell
    {
        public SeaCell(int x, int y, CellState state)
        {
            X = x;
            Y = y;
            this.State = state;
        }

        [ProtoMember(1)]
        public int X { get; set; }
        [ProtoMember(2)]
        public int Y { get; set; }
        [ProtoContract]
        public enum CellState
        {
            Free,
            OccupiedByShip,
            Attacked
        }
        [ProtoIgnore]
        public CellState State { get { return (CellState)CellStateInt; } set { CellStateInt = (int)value; } }
        [ProtoMember(3)]
        public int CellStateInt { get; set; }
    }
}
