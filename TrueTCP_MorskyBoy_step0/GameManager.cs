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

    /// <summary>
    /// ЗДЕСЬ ТОЛЬКО МЕТОДЫ
    /// </summary>
    internal class GameManager
    {

        public GameManager() 
        {
            
        }
        /// <summary>
        /// Преобразует результат проверки правил в текст и изменяет состояние классов Field и Game
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns>
        /// возвращает "OK" если расстановка прошла успешна;
        /// "error" - если правила нарушены
        /// </returns>
        public static string Rasstanovka(string stringList)
        {
            // TODO: десереализовать список кораблей

            /*
            stringList = stringList.Substring(5);
            Console.WriteLine(stringList); //DeBug на сервер 
            List<System.Drawing.Point> exemple = new List<System.Drawing.Point>();
            //"пример" точек который потом нужно "решить"
            exemple = stringTransformToList(stringList); // этот екземпл можно потом прокатить по правилам..
            //game.Rules(exemple) 
            bool IsCorrect = new Game().CheckGameRule_rasstanovka(exemple);
            if (!IsCorrect) return "Неправильная расстановка"; //далее просто переадим это клиенту а он там пусть сам гадает че не так сделал..
            return "OK";
            */
            return "OK";
        }
        /// <summary>
        /// Преобразует строку координат в Список точек с координатами
        /// </summary>
        /// <param name="stringList"></param>
        /// <returns>массив координат расстановленных клеток</returns>
        private static List<System.Drawing.Point> stringTransformToList(string stringList)
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
