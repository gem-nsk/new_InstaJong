using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using genField;
using UnityEngine.UI;
using UnityEngine;

namespace AStarPathfinder
{
    public class SettingsField
    {
        private int[,] fieldForFinder;
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

            fieldForFinder[start.X, start.Y] = 0;
            fieldForFinder[finish.X, finish.Y] = 0;


            List<Point> points = new List<Point>();

            points = CallFindPath();
            if (points == null) return null;

            if (GetCountTurn(points) <= 2)
            {
                foreach (Point point in points) Debug.Log(point);
                return points;
            }
            else
            {
                ClearField(points);
                foreach (Point point in points) Debug.Log(point);
                return PathCleaner(points);
            }
        }

        //передаем наше поле
        private void initField()
        {
            //инициализируем массив, в который будем загружать переработанную карту для поиска
            fieldForFinder = new int[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    fieldForFinder[i, j] = new int();

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {

                    if (field.array[i, j].getState() == 0) fieldForFinder[i, j] = 0;
                    if (field.array[i, j].getState() == 1 || field.array[i, j].getState() == 2)
                        fieldForFinder[i, j] = -2;

                }


        }

        private List<Point> CallFindPath()
        {
            return APathFinder.FindPath(fieldForFinder, start, finish);
        }

        private int GetCountTurn(List<Point> points)
        {
            int count = 0;
            for (int i = 0; i < points.Count - 2; i++)
            {
                if (Math.Abs(points[i].X - points[i + 2].X) == 1 &&
                    Math.Abs(points[i].Y - points[i + 2].Y) == 1)
                {
                    count++;
                }
            }
            return count;
        }

        private void IssuePenalties(List<Point> path, int barrier)
        {
            for (int i = 0; i < path.Count - 2; i++)
            {
                if ((Math.Abs(path[i].X - path[i + 2].X) == 1) &&
                    (Math.Abs(path[i].Y - path[i + 2].Y) == 1))
                {
                    if (barrier == 1)
                    {

                        if (fieldForFinder[path[i + 1].X, path[i + 1].Y] != 1)
                            fieldForFinder[path[i + 1].X, path[i + 1].Y] = 1;
                        else
                            fieldForFinder[path[i + 1].X, path[i + 1].Y] += 1;
                    }
                }
            }
        }

        private void SetFix()
        {
            if (start.X == finish.X)
            {
                int dY = Math.Abs(start.Y - finish.Y);
                for (int j = 0; j < dY; j++)
                {
                    if (start.Y != j && finish.Y != j)
                        fieldForFinder[start.X, j] = -2;
                }
                fieldForFinder[start.X, start.Y] = 0;
                fieldForFinder[finish.X, finish.Y] = 0;
            }
        }

        private void ClearField(List<Point> points)
        {
            foreach (Point point in points)
            {
                fieldForFinder[point.X, point.Y] = 0;
            }
            fieldForFinder[start.X, start.Y] = 0;
            fieldForFinder[finish.X, finish.Y] = 0;
        }

        private List<Point> PathCleaner(List<Point> points)
        {
            int countIter = 0;
            List<Point> firstPath = new List<Point>();
            firstPath = points;
            while (countIter < 30)
            {

                IssuePenalties(points, 1);
                points = CallFindPath();

                if (GetCountTurn(points) <= 2) return points;
                else countIter++;
                SetFix();

            }
            return null;
        }
    }
}

