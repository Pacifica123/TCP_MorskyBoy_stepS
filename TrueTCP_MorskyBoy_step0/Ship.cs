using System;
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
        public int type; //тип корабля
      //  int hp; //текущий размер корабля
        bool status; //цел/потоплен
        public List<SeaCell> cellList; // координаты ячеек корабля
        public enum vectoring
        {
            vertical,
            horizontal
        }
        public Ship.vectoring vec;
        public Ship(int _type, List<SeaCell> coordList, vectoring vectoring)
        {
            this.type = _type;
            this.cellList = coordList;
            //hp = type;
            this.vec = vectoring;
            this.status = true;
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
