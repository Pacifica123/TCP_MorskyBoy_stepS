﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server2.Engine
{
    [ProtoContract]
    public class Ship
    {
        [ProtoMember(1)]
        public int Type { get; }
        [ProtoMember(2)]
        public int Health { get; set; }
        [ProtoMember(3)]
        public List<SeaCell> ShipCells { get; set; }
        [ProtoMember(4)]
        public bool IsDestructed { get; set; }

        public Ship(int type)
        {
            this.Type = type;
            ShipCells = new List<SeaCell>();
            IsDestructed = false;
            Health = type;
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

        public bool ContainsCell(SeaCell targetCell)
        {
            foreach(SeaCell cell in ShipCells) 
            {
                if (cell.X == targetCell.X && cell.Y == targetCell.Y && cell.State == targetCell.State) return true;
            }
            return false;
        }
    }
}
