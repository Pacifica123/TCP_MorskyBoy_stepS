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
        public EndPoint id; //используем айпишник для идентификации игрока
        bool status; //готов/не готов

        public Player() 
        {
            status = false;
        }
    }
}
