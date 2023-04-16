using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Field
    {
        public List<List<SeaCell>> cells;
        public List<SeaCell> filledcells;
        public List<Ship> ships;

        public Field()
        {
            cells = new List<List<SeaCell>>();
            

            for (int row = 0; row <10; row++)
            {
                cells.Add(new List<SeaCell>()); //создаем строку ячеек
                for (int icell = 0; icell < 10; icell++) //заполняем строку свободными ячейками
                {
                    cells[row].Add(new SeaCell(new System.Drawing.Point(row, icell), SeaCell.state.free));
                }
            }
                
        }
        /// <summary>
        /// Заполняет объект текущего поля клетками на соответствующих координатах
        /// </summary>
        /// <param name="coordList"></param>
        public void fill(List<System.Drawing.Point> coordList)
        {
            filledcells = new List<SeaCell>();
            foreach (System.Drawing.Point p in coordList)
            {
                SeaCell cell = new SeaCell(p, SeaCell.state.filled_ship); // заполняем поле занятыми кораблями клетками
                filledcells.Add(cell);
                updateCells(p);
            }
        } 


        private bool IsFill(int col, int row)
        {
            bool yes = false;
            foreach (SeaCell cell in filledcells)
            {
                if (cell.coordinate.Y == row && cell.coordinate.X == col)
                {
                    yes = true;
                    return yes;
                }
            }
            return yes;
        }
        private void mapwrite()
        {
            bool yes = false;
            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    if (IsFill(col, row)) { Console.Write("■ "); } else { Console.Write("_ "); }
                }
                Console.WriteLine();
            }
        }

        public void newscanmap()
        {
            for(int row = 0; row<10; row++)
            {
                for(int col = 0; col<10; col++)
                {
                    // Если текущая клетка существует в списке заполненных клеток
                    if (filledcells.Exists(cell => cell.coordinate.Equals(cells[row][col].coordinate)) && filledcells.Exists(cell => cell.current_state.Equals(cells[row][col].current_state)))
                    {
                        // Создаем корабль
                        Ship ship = new Ship(1, new List<SeaCell>().Add(cells[row][col]), Ship.vectoring.horizontal);
                    }

                }
            }
        }

        #region SHIPCOUNT
        //public int CountShipsOfSize1()
        //{
        //    int count = 0;
        //    foreach (SeaCell cell in filledcells)
        //    {
        //        if (cell.current_state == SeaCell.state.filled_ship && ship.cellList.Count == 1)
        //        {
        //            count++;
        //        }
        //    }
        //    return count;
        //}

        //public int CountShipsOfSize2()
        //{
        //    int count = 0;
        //    foreach (SeaCell cell in filledcells)
        //    {
        //        if (cell.current_state == SeaCell.state.filled_ship && cellList.Count == 2)
        //        {
        //            bool isPartOfLargerShip = false;
        //            foreach (Ship ship in ships)
        //            {
        //                if (ship.cellList.Contains(cell) && ship.cellList.Count > 2)
        //                {
        //                    isPartOfLargerShip = true;
        //                    break;
        //                }
        //            }
        //            if (!isPartOfLargerShip)
        //            {
        //                count++;
        //            }
        //        }
        //    }
        //    return count;
        //}

        //public int CountShipsOfSize3()
        //{
        //    int count = 0;
        //    foreach (SeaCell cell in filledcells)
        //    {
        //        if (cell.current_state == SeaCell.state.filled_ship && cellList.Count == 3)
        //        {
        //            bool isPartOfLargerShip = false;
        //            foreach (Ship ship in ships)
        //            {
        //                if (ship.cellList.Contains(cell) && ship.cellList.Count > 3)
        //                {
        //                    isPartOfLargerShip = true;
        //                    break;
        //                }
        //            }
        //            if (!isPartOfLargerShip)
        //            {
        //                count++;
        //            }
        //        }
        //    }
        //    return count;
        //}

        //public int CountShipsOfSize4()
        //{
        //    int count = 0;
        //    foreach (SeaCell cell in filledcells)
        //    {
        //        if (cell.current_state == SeaCell.state.filled_ship && cellList.Count == 4)
        //        {
        //            bool isPartOfLargerShip = false;
        //            foreach (Ship ship in ships)
        //            {
        //                if (ship.cellList.Contains(cell) && ship.cellList.Count > 4)
        //                {
        //                    isPartOfLargerShip = true;
        //                    break;
        //                }
        //            }
        //            if (!isPartOfLargerShip)
        //            {
        //                count++;
        //            }
        //        }
        //    }
        //    return count;
        //}
        #endregion

        /// <summary>
        /// сканирует сразу всю карту для полноценного расставления всех кораблей и сбирает корабли в массив.
        /// </summary>
        public void scanmap()
        {
            mapwrite(); // удостоверимся что все данные пришли с клиента правильно


            /* // TODO: При добавлении..
             * 
             * При добавлении соседних клеток в список корабля, не учитывается, что клетка может уже принадлежать другому кораблю. 
             * Необходимо добавить проверку перед добавлением клетки в список.
             *
             */

            ships = new List<Ship>();



            // Сначала считаем корабли "1" и ">=2"
            // Затем среди второй категории считаем "3" и "4"
            /* COMMENTED
                        mapwrite(); // удостоверимся что все данные пришли с клиента правильно

                        // создаем список кораблей
                        ships = new List<Ship>();

                        foreach (SeaCell cell in filledcells)
                        {
                            SeaCell leftCell = null, rightCell = null, topCell = null, bottomCell = null;
                            // проверяем, есть ли заполненная клетка
                            // слева
                            if (cell.coordinate.X - 1 >= 0)
                            { leftCell = cells[cell.coordinate.X - 1][cell.coordinate.Y]; }
                            // справа
                            if (cell.coordinate.X + 1 < 10)
                            { rightCell = cells[cell.coordinate.X + 1][cell.coordinate.Y]; }
                            // сверху
                            if (cell.coordinate.Y - 1 >= 0)
                            { topCell = cells[cell.coordinate.X][cell.coordinate.Y - 1]; }
                            // снизу
                            if (cell.coordinate.Y + 1 < 10)
                            { bottomCell = cells[cell.coordinate.X][cell.coordinate.Y + 1]; }

                            //DeBug:
                            Console.WriteLine($"Левая ячейка: {leftCell?.coordinate.ToString() ?? "null"}; Правая: {rightCell?.coordinate.ToString() ?? "null"}; Верхняя: {topCell?.coordinate.ToString() ?? "null"}; Нижняя: {bottomCell?.coordinate.ToString() ?? "null"};");

                            // проверяем, есть ли две смежные клетки слева и справа или сверху и снизу:
                            // ГОРИЗОНТАЛЬНЫЙ ДВОЙНОЙ КОРАБЛЬ
                            if ((leftCell != null && leftCell.current_state == SeaCell.state.filled_ship) || (rightCell != null && rightCell.current_state == SeaCell.state.filled_ship))
                            {
                                // это корабль уровня 2
                                List<SeaCell> shipCells = new List<SeaCell>();
                                shipCells.Add(cell);
                                if (leftCell != null && leftCell.current_state == SeaCell.state.filled_ship)
                                {
                                    shipCells.Add(leftCell);
                                }
                                else if (rightCell != null && rightCell.current_state == SeaCell.state.filled_ship)
                                {
                                    shipCells.Add(rightCell);
                                }
                                Ship ship = new Ship(2, shipCells, Ship.vectoring.horizontal);
                                ships.Add(ship);
                            }
                            // ВЕРТИКАЛЬНЫЙ ДВОЙНОЙ КОРАБЛЬ
                            else if ((topCell != null && topCell.current_state == SeaCell.state.filled_ship) || (bottomCell != null && bottomCell.current_state == SeaCell.state.filled_ship))
                            {
                                // это корабль уровня 2
                                List<SeaCell> shipCells = new List<SeaCell>();
                                shipCells.Add(cell);
                                if (topCell != null && topCell.current_state == SeaCell.state.filled_ship)
                                {
                                    shipCells.Add(topCell);
                                }
                                else if (bottomCell != null && bottomCell.current_state == SeaCell.state.filled_ship)
                                {
                                    shipCells.Add(bottomCell);
                                }
                                Ship ship = new Ship(2, shipCells, Ship.vectoring.vertical);
                                ships.Add(ship);
                                // DeBug:
                                Console.WriteLine("Корабль отправляется для определения типа 2-4...");
                            }
                            // ОДИНОЧНЫЙ
                            else
                            {
                                // это корабль уровня 1
                                List<SeaCell> shipCells = new List<SeaCell>();
                                shipCells.Add(cell);
                                Ship ship = new Ship(1, shipCells, Ship.vectoring.horizontal);
                                ships.Add(ship);
                                // DeBug:
                                Console.WriteLine("Корабль был зарегистрирован как ЕДИНИЧНЫЙ");
                            }
                        }

                        // проверяем все корабли на наличие смежных ячеек для определения кораблей уровней 3 и 4
                        foreach (Ship ship in ships)
                        {
                            if (ship.type == 1)
                            {
                                continue;
                            }

                            int shipLength = ship.cellList.Count;
                            for (int i = 0; i < shipLength; i++)
                            {
                                SeaCell cell = ship.cellList[i];

                                // проверяем, есть ли две смежные клетки слева и справа или сверху и снизу
                                // (обнуляем переменные для новой цели)
                                SeaCell leftCell = null;
                                SeaCell rightCell = null;
                                SeaCell topCell = null;
                                SeaCell bottomCell = null;
                                if (i > 0)
                                {
                                    leftCell = ship.cellList[i - 1];
                                }
                                if (i < shipLength - 1)
                                {
                                    rightCell = ship.cellList[i + 1];
                                }
                                if (cell.coordinate.Y > 0)
                                {
                                    topCell = cells[cell.coordinate.X][cell.coordinate.Y - 1];
                                }
                                if (cell.coordinate.Y < 9)
                                {
                                    bottomCell = cells[cell.coordinate.X][cell.coordinate.Y + 1];
                                }
                                // это ГОРИЗОНТАЛЬНЫЙ корабль уровня 3
                                if ((leftCell != null && leftCell.current_state == SeaCell.state.filled_ship) || (rightCell != null && rightCell.current_state == SeaCell.state.filled_ship))
                                {

                                    if (ship.type < 3)
                                    {
                                        ship.type = 3;
                                    }
                                    //if (leftCell != null && leftCell.current_state == SeaCell.state.filled_ship)
                                    //{
                                    //    if (ship.vec == Ship.vectoring.vertical)
                                    //    {
                                    //        ship.vec = Ship.vectoring.horizontal;
                                    //    }
                                    //}
                                    //else 
                                    if (rightCell != null && rightCell.current_state == SeaCell.state.filled_ship)
                                    {
                                        if (ship.vec == Ship.vectoring.vertical)
                                        {
                                            ship.vec = Ship.vectoring.horizontal;
                                            Console.WriteLine("Корабль Внезапно сменил ориентацию! D: ");
                                        }
                                        ship.cellList.Add(rightCell);
                                    }
                                }
                                // это ВЕРТИКАЛЬНЫЙ корабль уровня 3
                                else if ((topCell != null && topCell.current_state == SeaCell.state.filled_ship) || (bottomCell != null && bottomCell.current_state == SeaCell.state.filled_ship))
                                {
                                    if (ship.type < 3)
                                    {
                                        ship.type = 3;
                                    }
                                    //if (topCell != null && topCell.current_state == SeaCell.state.filled_ship)
                                    //{
                                    //    if (ship.vec == Ship.vectoring.horizontal)
                                    //    {
                                    //        ship.vec = Ship.vectoring.vertical;
                                    //    }
                                    //}
                                    //else 
                                    if (bottomCell != null && bottomCell.current_state == SeaCell.state.filled_ship)
                                    {
                                        if (ship.vec == Ship.vectoring.horizontal)
                                        {
                                            ship.vec = Ship.vectoring.vertical;
                                            Console.WriteLine("Корабль Внезапно сменил ориентацию! D: ");
                                        }
                                        ship.cellList.Add(bottomCell);
                                    }
                                }

                                // проверяем, есть ли три смежные клетки слева и справа или сверху и снизу
                                // это ГОРИЗОНТАЛЬНЫЙ корабль уровня 4
                                if (leftCell != null && rightCell != null && leftCell.current_state == SeaCell.state.filled_ship && rightCell.current_state == SeaCell.state.filled_ship)
                                {
                                    if (ship.type < 4)
                                    {
                                        ship.type = 4;
                                    }
                                    if (ship.vec == Ship.vectoring.vertical)
                                    {
                                        ship.vec = Ship.vectoring.horizontal;
                                        Console.WriteLine("Корабль внезапно сменил ориентацию при переходе с 3 на 4");
                                    }
                                    ship.cellList.Add(leftCell);
                                    ship.cellList.Add(rightCell);
                                }
                                // это ВЕРТИКАЛЬНЫЙ корабль уровня 4
                                else if (topCell != null && bottomCell != null && topCell.current_state == SeaCell.state.filled_ship && bottomCell.current_state == SeaCell.state.filled_ship)
                                {
                                    if (ship.type < 4)
                                    {
                                        ship.type = 4;
                                    }
                                    if (ship.vec == Ship.vectoring.horizontal)
                                    {
                                        ship.vec = Ship.vectoring.vertical;
                                        Console.WriteLine("Корабль внезапно сменил ориентацию при переходе с 3 на 4");
                                    }
                                    ship.cellList.Add(topCell);
                                    ship.cellList.Add(bottomCell);
                                }
                            }
                        }
                        shipsetStatWrite();
                        Console.WriteLine($"Завершение функции scanmap");
            */
        }

        /// <summary> 
        /// обновляем поле, заносим инфу о заполненных клетках
        /// </summary>
        public void updateCells(System.Drawing.Point p)
        {
            foreach (List<SeaCell> row in cells)
                foreach (SeaCell c in row)
                    if (c.coordinate == p) { c.current_state = SeaCell.state.filled_ship; };
        }
        public void shipsetStatWrite()
        {
            Console.WriteLine("Статистика о кораблях: ");
            int x4count = ships.Count(sh4 => sh4.type == 4);
            int x3count = ships.Count(sh3 => sh3.type == 3);
            int x2count = ships.Count(sh2 => sh2.type == 2);
            int x1count = ships.Count(sh1 => sh1.type == 1);

            Console.WriteLine($"Кораблей четверок: {x4count}");
            Console.WriteLine($"Кораблей троек: {x3count}");
            Console.WriteLine($"Кораблей двоек: {x2count}");
            Console.WriteLine($"Кораблей однерок: {x1count}");
        }
    }
}
