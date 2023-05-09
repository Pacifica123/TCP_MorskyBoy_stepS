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
        public string game_id { get; set; } //для стека игр на сервере
        public Player player1 { get; set; }
        public Player player2 { get; set; }
        public Player currentPlayer { get; set; } //текущий игрок чей ход (для режима атаки)
        public bool IsFilledGame { get; set; } //оба игрока подключились к текущей игре и нужно создать новую
        public bool GameStarted { get; set; } // флаг начала игры (активируется по завершению расстановки у каждого из игроков)
        public bool GameOver { get; set; } // флаг конца игры
        public Field buffSea { get; set; } // буфер для поля для передачи игроку затем

        public Game(string id)
        {
            this.game_id = id; // ip первого подключившегося игрока + его порт
            // старт игры, создание игроков, расстановка кораблей
            //расстановка кораблей происходит сразу же, получая массив с клиента
            //если расстановка неверная, сервер посылает клиента переделать расстановку (обновить массив)
            
        }
        /// <summary>
        /// Проверяет возможность хода и обновляет состояние поля (СМ: подробнее)
        /// </summary>
        /// <param name="coord"> координата куда ударил игрок</param>
        public void Turn(Point coord)
        {
            //вызывается при ходе игрока
            //если игрок попал, то он получает ещё одну возможность хода
            //когда все корабли одного из игроков потоплены - игра завершается
        }
        /// <summary>
        /// сменяет ход текущего игрока на другого
        /// </summary>
        public void ChangePlayer()
        {
            // 
        }
        /// <summary>
        /// Выводит информацию о победителе
        /// </summary>
        public void EndGame() 
        {
            // 
        }
        /// <summary>
        /// Проверка 3 правил расстановки (кол-во кораблей, углы, тип корабле не больше 4)
        /// </summary>
        /// <param name="coordList"></param>
        /// <returns>По умолчанию false на всякий случай</returns>
        public bool CheckGameRule_rasstanovka(List<Ship> ships)
        {

            buffSea = new Field();
            
            buffSea.fill(ships); // поставит нужные ячейки в соответствии с кораблями
            buffSea.ships = ships;
            
            return ShipCorrect(buffSea);
        }

        /// <summary>
        /// Проверяет, правильное ли соотношение кол-ва кораблей (4х1 3х2 2х3 1х4)
        /// </summary>
        /// <returns>true если все нормально</returns>
        private bool ShipCorrect(Field sea)
        {
            bool result = false; //по умолчанию на всякий
            buffSea.shipsetStatWrite();
            return (buffSea.shipsetStatReturn()[0] == 4 && buffSea.shipsetStatReturn()[1] == 3 && buffSea.shipsetStatReturn()[2] == 2 && buffSea.shipsetStatReturn()[3] == 1);
        }
      
    }
}
