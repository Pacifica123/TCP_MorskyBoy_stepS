using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    internal class Player
    {
        public EndPoint id { get; set; } //используем айпишник для идентификации игрока
        public bool status { get; set; }//готовв/не готов
        public Field Sea { get; set; }
        public int game_id { get; set; } // id игры к которой принадлежит клиент
        public Player() 
        {
            status = false;
        }
    }
}
