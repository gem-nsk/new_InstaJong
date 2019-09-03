using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using genField;

namespace AStarPathfinder
{
    public class SettingsField
    {
        private CellField[,] fieldForFinder;
        private Point start;
        private Point finish;
        private int width;
        private int height;

        private Field field;
        public SettingsField(Field field, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.field = field;
            initField();
        }

        public List<Point> LittlePathfinder(Point start, Point finish)
        {
            this.start = start;
            this.finish = finish;

            fieldForFinder[start.X, start.Y].value = 0;
            fieldForFinder[finish.X, finish.Y].value = 0;

            List<Point> points = new List<Point>();
            bool correctPathIsFound = false;
            int countIter = 0;
            while (correctPathIsFound == false)
            {
                points = CallFindPath();
                if (points == null) return null;
                //если путь состоит из двух и менее поворотов,
                //то выходим из цикла
                //если путь стотит более чем из двух поворотов,
                //то очищаем поле и выбираем другой маршрут
                if (GetCountTurnes(points) <= 2)
                {
                    for (int i = 0; i < points.Count() - 1; i++)
                        fieldForFinder[points[i + 1].X, points[i + 1].Y].value = -1;
                    correctPathIsFound = true;
                }
                else ClearField(points);
                countIter++;
                if (countIter == 25) return null;
                

            }

            return points;
        }

        //передаем наше поле
        private void initField()
        {
            //инициализируем массив, в который будем загружать переработанную карту для поиска
            fieldForFinder = new CellField[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    fieldForFinder[i, j] = new CellField();

            /*
             * Перекачиваем нужную нам информацию карты, а именно State
             * если state = 1 или -2, то значит это препятстиве,
             * через которое поиск не должен ходить
             * если state 0, то это пустое пространство, через которое можно ходить
             * p.s state в массиве fieldForFinder не является тем же самым, что и в массиве field.
             * B этом массиве state нужен чтобы отслеживать количество поворотов
             */
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {

                    if (field.array[i, j].getState() == 0) fieldForFinder[i, j].value = 0;
                    if (field.array[i, j].getState() == 1 || field.array[i, j].getState() == 2)
                        fieldForFinder[i, j].value = -2;

                    fieldForFinder[i, j].state = null;
                }
        }

        private List<Point> CallFindPath ()
        {
            return APathFinder.FindPath(fieldForFinder, start, finish);
        }

        private int GetCountTurnes(List<Point> points)
        {
            int countTurnes = 0;
            for(int i = 1; i < points.Count()-1; i++)
                if (fieldForFinder[points[i].X, points[i].Y].state != fieldForFinder[points[i + 1].X, points[i + 1].Y].state)
                    countTurnes++;
            //Console.WriteLine(countTurnes);
            return countTurnes;
        } 

        private void ClearField(List<Point> points)
        {
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {

                    if (field.array[i, j].getState() == 0) fieldForFinder[i, j].value = 0;
                    if (field.array[i, j].getState() == 1 || field.array[i, j].getState() == 2)
                        fieldForFinder[i, j].value = -2;
                }

            for (int i = 0; i < points.Count() - 1; i++)
                if (fieldForFinder[points[i].X, points[i].Y].state != fieldForFinder[points[i + 1].X, points[i + 1].Y].state)
                    fieldForFinder[points[i + 1].X, points[i + 1].Y].value = 1;

            for(int i = 0; i < height; i++)
                for(int j = 0; j < width; j++)
                    fieldForFinder[i, j].state = null;

            fieldForFinder[start.X, start.Y].value = 0;
            fieldForFinder[finish.X, finish.Y].value = 0;
        }

        public void PrintField(int trigger)
        {
            Console.WriteLine();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (fieldForFinder[i, j].value == -2) Console.Write("{0,3}", "X");
                    else if (trigger == 0) { if (fieldForFinder[i, j].value == -1) Console.Write("{0,3}", "+"); }
                    else if (trigger == 1) { Console.Write("{0,3}", fieldForFinder[i, j].state); }
                    else if (i == start.X && j == start.Y) Console.Write("{0,3}", "S");
                    else if (i == finish.X && j == finish.Y) Console.Write("{0,3}", "F");
                    else Console.Write("{0,3}", fieldForFinder[i, j].value);

                }
                Console.WriteLine();
            }

        }
    }
}
