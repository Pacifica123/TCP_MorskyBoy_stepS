using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    [ProtoContract]
    public class Sea
    {
        [ProtoMember(1)]
        public List<SeaCell> SeaCells { get; set; }
        [ProtoMember(2)]
        public List<Ship> Ships { get; set; }

        public Sea()
        {
            SeaCells = new List<SeaCell>();
            Ships = new List<Ship>();
        }
    }
}
