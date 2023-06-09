﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    internal class Ship
    {
        public int type { get; set; } //тип корабля
        public bool status { get; set; } //цел/потоплен
        public List<SeaCell> cellList { get; set; } // координаты ячеек корабля
        public enum vectoring
        {
            vertical,
            horizontal
        }
        public Ship.vectoring vec { get; set; }
        public Ship(int type, List<SeaCell> coordList, vectoring vectoring)
        {
            this.type = type;
            this.cellList = coordList;
            vec = vectoring;
            status = true;
        }
        public bool DamageAndCheck(Point demagedCell)
        {
            // операция не прошла т.к. это уже использованное поле
            if (cellList[demagedCell.X*10+demagedCell.Y].current_state == SeaCell.state.broken) { return false; } 
            type--;
            foreach(SeaCell cell in cellList)
            {
                if (cell.coordinate == demagedCell) cell.current_state = SeaCell.state.broken;
            }
            if (type == 0) { status = false; }
            return true; // операция удалась успешно
        }
    }
}
