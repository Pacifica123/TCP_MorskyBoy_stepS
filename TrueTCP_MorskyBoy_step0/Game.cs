﻿using System;
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
        public bool CheckGameRule_rasstanovka(List<System.Drawing.Point> coordList)
        {

            Field buffSea = new Field();
            buffSea.fill(coordList); // поставит нужные ячейки
            buffSea.scanmap(); // соберет ячейки в корабли
            // TODO: далее проверка стандартная на 1)корабли 2)углы 3)тип корабля не больше 4 
            if (AngleCorrect(buffSea) && ShipCorrect(buffSea) && TypeCorrect(buffSea)) return true;

            return false; // по умолчанию мы не проходим по правилам (на всякий случай)
        }
        /// <summary>
        /// Проверяет нет ли кораблей, состоящих более чем из 4х клеток
        /// </summary>
        /// <returns>true если все нормально</returns>
        private bool TypeCorrect(Field sea)
        {
            // TODO: для функции нужно допилить scanmap 
            bool result = false; //по умолчанию на всякий
            return result;
        }
        /// <summary>
        /// Проверяет, правильное ли соотношение кол-ва кораблей (4х1 3х2 2х3 1х4)
        /// </summary>
        /// <returns>true если все нормально</returns>
        private bool ShipCorrect(Field sea)
        {
            // TODO: для функции нужно допилить scanmap 
            bool result = false; //по умолчанию на всякий
            return result;
        }
        /// <summary>
        /// Проверяет, нет ли клеток, сопрекасающихся по углам или друг к другу
        /// </summary>
        /// <returns>true если все нормально</returns>
        private bool AngleCorrect(Field sea)
        {
            bool result = true;
            for (int i = 0; i < sea.filledcells.Count; i++)
            {
                int x = sea.filledcells[i].coordinate.X;
                int y = sea.filledcells[i].coordinate.Y;
                // Проверяем, есть ли занятые клетки сверху-слева, сверху-справа, снизу-слева, снизу-справа
                // В случае нахождения занятой клетки рядом с текущей, возвращаем false
                if ((y > 0 && x > 0 && sea.cells[y - 1][x - 1].current_state != SeaCell.state.free) ||
                (y > 0 && x < sea.cells[y].Count - 1 && sea.cells[y - 1][x + 1].current_state != SeaCell.state.free) ||
                (y < sea.cells.Count - 1 && x > 0 && sea.cells[y + 1][x - 1].current_state != SeaCell.state.free) ||
                (y < sea.cells.Count - 1 && x < sea.cells[y].Count - 1 && sea.cells[y + 1][x + 1].current_state != SeaCell.state.free) ||
                (y > 0 && sea.cells[y - 1][x].current_state != SeaCell.state.free) ||
                (y < sea.cells.Count - 1 && sea.cells[y + 1][x].current_state != SeaCell.state.free) ||
                (x > 0 && sea.cells[y][x - 1].current_state != SeaCell.state.free) ||
                (x < sea.cells[y].Count - 1 && sea.cells[y][x + 1].current_state != SeaCell.state.free))
                {
                    result = false;
                    break; //досрочно прерываем цикл, если найдена занятая клетка рядом
                }
            }
            return result;
        }
    }
}
