using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Ship
    {
        int type; //тип корабля
        int hp; //текущий размер корабля
        bool status; //цел/потоплен
        List<SeaCell> cellList; // координаты ячеек корабля
        public enum vectoring
        {
            vertical,
            horizontal
        }
        public Ship.vectoring vec;
        public Ship(int type, List<SeaCell> coordList, vectoring vectoring)
        {
            this.type = type;
            this.cellList = coordList;
            hp = type;
            vec = vectoring;
        }
        public bool DamageAndCheck(Point demagedCell)
        {
            // операция не прошла т.к. это уже использованное поле
            if (cellList[demagedCell.X*10+demagedCell.Y].current_state == SeaCell.state.broken) { return false; } 
            hp--;
            foreach(SeaCell cell in cellList)
            {
                if (cell.coordinate == demagedCell) cell.current_state = SeaCell.state.broken;
            }
            if (hp == 0) { status = false; }
            return true; // операция удалась успешно
        }
    }
}
