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
    }
}
