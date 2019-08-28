using System;
using System.Collections.Generic;

namespace genField
{
    public class Field
    {
        public int countTypes { get; set; }
        public int countImageInType { get; set; }
        public int widthField { get; set; }
        public int heightField { get; set; }

        public Cell[,] array;

        private int countElements = 0;

        public Field() { }

        public Field(int width, int height)
        {
            this.widthField = width;
            this.heightField = height;
        }
        public Field(int width, int height, int countTypes, int countImageInType)
        {
            this.widthField = width;
            this.heightField = height;
            this.countTypes = countTypes;
            this.countImageInType = countImageInType;
        }

        public int initField(bool allStateOne)
        {
            // инициализация поля игры, установка id каждой ячейке
            array = new Cell[heightField, widthField];
            if (widthField <= 0 || heightField <= 0) return -1;
            int newId = 1;
            for (int i = 0; i < heightField; i++)
                for (int j = 0; j < widthField; j++)
                {
                    array[i, j] = new Cell();
                    if (i >= 1 && j >= 1 && i < heightField - 1 && j < widthField - 1)
                    {
                        array[i, j].setId(newId);
                        newId += 1;
                        countElements++;
                    }
                    else
                    {
                        array[i, j].setId(0);
                        array[i, j].setState(2);
                    }

                }
            //установка всех ячеек в state 1
            if (allStateOne)
            {
                for (int i = 2; i < heightField - 2; i++)
                    for (int j = 2; j < widthField - 2; j++)
                        array[i, j].setState(1);
            }
            Console.WriteLine(countElements);

            return 0;
        }
        // раздача randomNum для каждой ячейки
        public int generateField()
        {
            Random random = new Random();
            int rInt;
            int maxRange = countElements;
            
            for (int n = 0; n < countTypes; n++)
            {
                for (int k = 0; k < countImageInType;)
                {
                    //rInt - переменная, которая получает случайный id ячейки
                    rInt = random.Next(1, maxRange);
                    var coords = findCoordsById(rInt);

                    if(coords.i != 0 && coords.j != 0 &&
                        array[coords.i, coords.j].getState() == 1 &&
                        array[coords.i, coords.j].getRandomNum() == 0)
                    {
                        array[coords.i, coords.j].setRandomNum(n + 1);
                        k++;
                    }
                    else
                    {
                        rInt = random.Next(1, maxRange);
                    }

                }
            }
            return 0;
        }

        public int generateField(List<Tuple<int, int>> aviableCells)
        {
            Random random = new Random();
            int rInt;
            int maxRange = countElements;
            //цикл по доступным ячейкам
            for (int i = 0; i < aviableCells.Count;)
            {
                //берем случайный ID ячейки на поле
                rInt = random.Next(0, maxRange);
                //находим координаты ячейки в массиве по ID
                var coords = findCoordsById(rInt);
                //если у найденной ячейки state = 1, то помещаем туда новый RandomNum
                if (coords.i != 0 && coords.j != 0 &&
                        array[coords.i, coords.j].getState() == 1 &&
                        array[coords.i, coords.j].getRandomNum() == 0)
                {
                    var aviableCellRNum = aviableCells[i].Item2;
                    array[coords.i, coords.j].setRandomNum(aviableCellRNum);
                    i++;
                }
            }
            return 0;
        }

        public Field refreshField(Field field)
        {
            var map = field.array;
            //инициализация списка для добавления доступных для перемешивания ячеек
            List<Tuple<int, int>> aviableCells = new List<Tuple<int, int>>();
            //цикл по карте в поисках доступных ячеек
            for (int i = 0; i < field.heightField; i++)
            {
                for (int j = 0; j < field.widthField; j++)
                {
                    //если нашли ячейку со статусом 1 - добавляем в список
                    if (map[i, j].getState() == 1)
                    {
                        var aviableCell = Tuple.Create(map[i, j].getId(), map[i, j].getRandomNum());
                        aviableCells.Add(aviableCell);
                        map[i, j].setRandomNum(0);
                    }
                }
            }
            //после того как нашли доступные ячейки
            //перемешиваем их между собой, сохраняя их количество
            generateField(aviableCells);
            return field;
        }

        //функция поиска позиции ячейки массива по id
        public (int i, int j) findCoordsById(int id)
        {
            for (int i = 1; i < heightField-1; i++)
            {
                for (int j = 1; j < widthField-1; j++)
                {
                    
                    if (array[i, j].getId() == id)
                        return (i, j);
                }
            }

            return (0, 0);
        }

        public int[,] toIntArray(int num)
        {
            int[,] intArray = new int[heightField, widthField];
            switch (num)
            {
                case 0:
                    {
                        for (int i = 0; i < heightField; i++)
                        {
                            for (int j = 0; j < widthField; j++)
                            {
                                if (array[i, j].getState() == 2 || array[i, j].getState() == 1)
                                    intArray[i, j] = -4;
                                if (array[i, j].getState() == 0)
                                    intArray[i, j] = -1;
                            }
                        }
                        break;
                    }
                default: break;
            };

            return intArray;

        }

        // функция вывода поля на экран

        public void printField()
        {
            Console.Write("\n");
            for (int i = 0; i < heightField; i++)
            {
                for (int j = 0; j < widthField; j++)
                {
                    if (array[i, j].getState() == 1) Console.BackgroundColor = ConsoleColor.Red;
                    else if (array[i, j].getState() == 0) Console.BackgroundColor = ConsoleColor.Blue;
                    else if (array[i, j].getState() == 2) Console.BackgroundColor = ConsoleColor.Black;
                    else Console.BackgroundColor = ConsoleColor.Black;

                    String output = String.Format("{0,3}", array[i, j].getRandomNum().ToString());

                    Console.Write(output);
                }
                Console.Write("\n");
            }
        }
        
    }
}
