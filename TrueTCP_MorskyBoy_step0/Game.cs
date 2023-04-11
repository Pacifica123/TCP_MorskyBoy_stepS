using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Game
    {
        public Player player1, player2;
        public Player currentPlayer; //текущий игрок чей ход (для режима атаки)
        public bool GameStarted; // флаг начала игры (активируется по завершению расстановки у каждого из игроков)
        public bool GameOver; // флаг конца игры

        public Game()
        {
            // старт игры, создание игроков, расстановка кораблей
            //расстановка кораблей происходит сразу же, получая массив с клиента
            //если расстановка неверная, сервер посылает клиента переделать расстановку (обновить массив)
            player1 = new Player(); player2 = new Player();
        }
        public void Turn(Point coord)
        {
            //вызывается при ходе игрока
            //проверяет возможность хода и обновляет состояние поля
            //если игрок попал, то он получает ещё одну возможность хода
            //когда все корабли одного из игроков потоплены - игра завершается
        }
        public void ChangePlayer()
        {
            // сменяет ход, т.е. текущего игрока на другого
        }
        public void EndGame() 
        {
            // Выводит информацию о победителе
        }
        public bool CheckGameRule_rasstanovka(List<System.Drawing.Point> coordList)
        {

            Field buffSea = new Field();
            buffSea.fill(coordList); // поставит нужные ячейки
            buffSea.scanmap(); // соберет ячейки в корабли
            // TODO: 
            // далее проверка стандартная на 1)корабли 2)углы 3)тип корабля не больше 4
            return false; // по умолчанию мы не проходим по правилам (на всякий случай)
        }
    }
}
