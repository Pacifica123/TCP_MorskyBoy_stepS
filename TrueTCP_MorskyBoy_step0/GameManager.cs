using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    // управляет процессом игры
    // создает объекты и запсукает игру для каждого игрока
    internal class GameManager
    {

        public GameManager() 
        {
            //опасно, т.к. можно забыть что поля отсюда использовать нельзя...
            // (как вариант, можно для безопасности передовать текущее состояние игры по функциям в параметры)
            Game game = new Game(); // это класс в основном будет возвращать bool (вернее только использоваться)
        }
        // возвращает "OK" если расстановка прошла успешна
        // "error" - если правила нарушены
        // также изменяет состояние классов Field и Game
        public string Rasstanovka(string stringList)
        {
            stringList = stringList.Substring(5);
            Console.WriteLine(stringList); //DeBug
            List<System.Drawing.Point> exemple = new List<System.Drawing.Point>();
            exemple = stringTransformToList(stringList); // этот екземпл можно потом прокатить по правилам..
            //game.Rules(exemple) 
            return "OK";
        }
        private List<System.Drawing.Point> stringTransformToList(string stringList)
        {
            List<System.Drawing.Point> exemple = new List<System.Drawing.Point>();
            List<int> buff = new List<int>();
            foreach (char ch in stringList)
            {
                if (Char.IsDigit(ch)) buff.Add(Int32.Parse(ch.ToString()));
            }
            for (int i = 0; i < buff.Count-1; i+=2)
            {
                System.Drawing.Point p = new System.Drawing.Point(buff[i], buff[i+1]);
                exemple.Add(p);
            }
            return exemple;
        }
    }
}
