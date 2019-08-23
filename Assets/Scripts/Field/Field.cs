using System;

namespace genField
{
    public class Field
    {
        private int countTypes;
        private int countImageInType;
        private int widthField;
        private int heightField;
        public Cell[,] array;

        private int countElements = 0;

        public Field(int width, int height)
        {
            this.widthField = width;
            this.heightField = height;

        }

        public int initField(int countTypes, int countImageInType, bool allStateOne)
        {
            this.countTypes = countTypes;
            this.countImageInType = countImageInType;
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
        //функция поиска позиции ячейки массива по id
        public (int i, int j) findCoordsById(int id)
        {
            for (int i = 1; i < heightField - 1; i++)
            {
                for (int j = 1; j < widthField - 1; j++)
                {

                    if (array[i, j].getId() == id)
                        return (i, j);
                }
            }

            return (0, 0);
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
                    else if (array[i, j].getState() == 2) Console.BackgroundColor = ConsoleColor.Cyan;
                    else Console.BackgroundColor = ConsoleColor.Black;

                    String output = String.Format("{0,3}", array[i, j].getRandomNum().ToString());

                    Console.Write(output);
                }
                Console.Write("\n");
            }
            Console.ReadKey(true);
            Console.Clear();

        }

    }
}
