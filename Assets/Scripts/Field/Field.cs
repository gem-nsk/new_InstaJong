using System;
using System.Collections.Generic;
using UnityEngine;

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

        public Field(int height, int width) { widthField = width; heightField = height; }

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

                    array[i, j].setCoords(i, j);
                    array[i, j].setId(newId);



                    newId += 1;
                    countElements++;

                }

            if (allStateOne)
            {
                for (int i = 1; i < heightField - 1; i++)
                    for (int j = 1; j < widthField - 1; j++)
                        array[i, j].setState(1);
            }

            return 0;
        }
        // раздача randomNum для каждой ячейки
        public int generateField()
        {
            System.Random random = new System.Random();
            int rInt;
            int maxRange = countElements;
            for (int n = 0; n < countTypes; n++)
            {
                for (int k = 0; k < countImageInType;)
                {
                    //rInt - переменная, которая получает случайный id ячейки
                    rInt = random.Next(1, maxRange);
                    var coords = findCoordsById(rInt);

                    if (coords.i != 0 && coords.j != 0 &&
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
            System.Random random = new System.Random();
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

            foreach (Cell cell in array)
            {
                if (cell.getId() == id) return cell.getCoords();
            }

            return (0, 0);
        }

        //функция поиска позиции ячейки массива по координатам
        public int findIdByCoords(int i, int j)
        {
            return array[i, j].getId();
        }

        public int[,] GetMatrix()
        {
            int[,] matrix = new int[heightField, widthField];
            for (int y = 0; y < widthField; y++)
            {
                matrix[0, y] = matrix[heightField - 1, y] = 0;
            }
            for (int x = 0; x < heightField; x++)
            {
                matrix[x, 0] = matrix[x, widthField - 1] = 0;
            }

            for (int i = 0; i < heightField; i++)
            {
                for (int j = 0; j < widthField; j++)
                {
                    matrix[i, j] = array[i, j].getRandomNum();
                }
            }
            return matrix;
        }

        public Cell[,] DoPair((int row, int col) cell1, (int row, int col) cell2, Cell[,] array)
        {
            var matrix = array;

            matrix[cell1.row, cell1.col].setState(0);
            matrix[cell1.row, cell1.col].setRandomNum(0);

            matrix[cell2.row, cell2.col].setState(0);
            matrix[cell2.row, cell2.col].setRandomNum(0);

            int firstMoveRow = cell1.row;
            int secondMoveRow = cell2.row;
            if (cell1.col == cell2.col)
            {
                firstMoveRow = cell1.row > cell2.row ? cell1.row : cell2.row;
                secondMoveRow = cell1.row > cell2.row ? cell2.row : cell1.row;
            }
            int cell1Col = cell1.col;
            for (int i = firstMoveRow; i < matrix.GetLength(0) - 2; i++)
            {
                matrix[i, cell1Col].setRandomNum(matrix[i + 1, cell1Col].getRandomNum());
                matrix[i, cell1Col].setState(matrix[i + 1, cell1Col].getState());
            }

            matrix[matrix.GetLength(0) - 2, cell1.col].setState(0);
            matrix[matrix.GetLength(0) - 2, cell1.col].setRandomNum(0);

            int cell2Col = cell2.col;
            for (int i = secondMoveRow; i < matrix.GetLength(0) - 2; i++)
            {
                matrix[i, cell2Col].setRandomNum(matrix[i + 1, cell2Col].getRandomNum());
                matrix[i, cell2Col].setState(matrix[i + 1, cell2Col].getState());
            }

            matrix[matrix.GetLength(0) - 2, cell2.col].setState(0);
            matrix[matrix.GetLength(0) - 2, cell2.col].setRandomNum(0);


            return matrix;
        }

    }
}
