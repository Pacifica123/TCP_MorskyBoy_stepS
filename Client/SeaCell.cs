using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class SeaCell
    {
        //содержит информацию о ее состоянии (свободна/занята кораблем/подбита) - мб структурой описать её?
        public System.Drawing.Point coordinate;
        public enum state
        {   
            free,
            filled_ship,
            broken
        }
        public SeaCell.state current_state;
        public SeaCell(System.Drawing.Point coordinate, state state)
        {
            this.coordinate = coordinate;
            this.current_state = state;
        }
    }
}
