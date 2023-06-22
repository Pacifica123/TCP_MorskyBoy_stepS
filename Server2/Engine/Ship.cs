using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    public class Ship
    {
        public int Type { get; }
        public int Health { get; set; }
        public List<SeaCell> ShipCells { get; set; }
        public bool IsDestructed { get; set; }

        public Ship(int type)
        {
            this.Type = type;
            ShipCells = new List<SeaCell>();
            IsDestructed = false;
        }
        public void Damage()
        {
            Health--;
            if (Health <= 0)
            {
                // Корабль уничтожен
                IsDestructed = true;
            }
        }
    }
}
