using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
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
        //public static List<Ship> shipsBuff;
        public GameManager() 
        {
           // shipsBuff = new List<Ship>();

        }
        /// <summary>
        /// Преобразует результат проверки правил в текст и изменяет состояние классов Field и Game
        /// </summary>
        /// <param name="stringList">строка для десериализации</param>
        /// <returns>
        /// возвращает "OK" если расстановка прошла успешна;
        /// "error" - если правила нарушены
        /// </returns>
        public static string Rasstanovka(string stringList, Game transportedGame)
        {
            List<Ship> shipList = JsonConvert.DeserializeObject<List<Ship>>(stringList);
           
            bool IsCorrect = transportedGame.CheckGameRule_rasstanovka(shipList);
            if (!IsCorrect) return "Incorrect set ships!!!"; //далее просто передадим это клиенту а он там пусть сам гадает че не так сделал..
            //Если все хорошо
            transportedGame.currentPlayer.Sea = transportedGame.buffSea;
            transportedGame.currentPlayer.status = true;
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
