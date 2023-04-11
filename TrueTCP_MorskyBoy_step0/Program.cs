using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Сервер включается...");
            //Создаем прослушиватель Сервера
            // сокет для конкретного Клиента
            // Засовываем все это в ООП...
            TheNetworkManager networkManager = new TheNetworkManager(Dns.GetHostByName(Dns.GetHostName()).AddressList[0], 8888);
            networkManager.Start();
        }
        // РЕАЛИЗАЦИЯ ЛОГИКИ ИГРЫ
        /*
         *  1. Одна функция на обработку всех ответов/запросов Сервером - ProcessMessage
         *  2. В зависимости от запроса ответ:
         *      а) меняет состояние классов игры
         *      б) дает положительный/отрицательный ответ на соблюдение правил
         *      в) возвращает "ОК" во всех остальных случаях (поэтому все негативные варианты обрабатывается в явном виде)
         *  3. Функция НЕ всегда возвращает сообщение-код как ответ на который будет реагировать Клиент, для этого и предусмотрен "ОК",
         *  на которое Клиент видет себя так как будто ничего не произошло
         *  4. networkManager - самый внешний/общий механизм, в который входит GameManager, контролирующий Game с доступом к остальным классам
         */

    }
}