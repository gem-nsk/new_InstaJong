using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using genField;
using AStarPathfinder;
using System.Drawing;

namespace genField
{
    public class PathParser
    {
        private Field field;
        public List<Point> points;

        // флаг, кторый обозначает найден ли пользователем сохраненный путь
        public bool PathFound { get; set; }
        // флаг, который обозначает существует еще ход на карте или нет
        public bool PathExists { get; set; }

        public (int idFirst, int idSecond) path;

        //Конструктор класса
        public PathParser()
        {
            /*
             * Инициализируем подключаемы модули: 
             *  - генерация карты
             *  - волновой метод поиска пути
             */

        }

        //функция поиска возможного хода
        public int parse(Field field)
        {
            this.field = field;
            PathFound = false;
            PathExists = false;

            //флаг для обозначения нашли возможный ход или нет
            bool findFlag = false;
            /*
             * coordsCell - переменная, которая хранит координаты проверяемой ячейки.
             * Начальные координаты (2,2) потому что (0,0) это рамка,
             * а (1,1) это начальное незаполненное поле для поиска пути.
             */
            (int x, int y) coordsCell = (0, 0);
            

            if(PathExists == false)
            {
                //ищем, пока не будет найден возможный ход
                while (findFlag == false)
                {
                    //берем id и randomNum 
                    Debug.Log("#pathParser: " + coordsCell);
                    //int id = field.array[coordsCell.x, coordsCell.y].getId();
                    int randomNum = field.array[coordsCell.x, coordsCell.y].getRandomNum();

                    //ищем все ячейки с одинаковым типом и записываем в список
                    var foundIdCells = searchSimilarCells(randomNum);
                    Debug.Log("#pathParser/foundCells: " + foundIdCells.Count);
                    /*
                     * Процесс поиска происходит следующим образом
                     * перебором среди списка однотипных ячеек ищем пути
                     * если путь есть, то сохраняем этот путь и завершаем поиск
                     * если нет, то берем следующий тип и проводим те же операции
                     */
                    for (int i = 0; i < foundIdCells.Count - 1; i++)
                    {

                        (int x, int y) coordsStart = foundIdCells[i];
                        (int x, int y) coordsFinish = foundIdCells[i+1];
                        //Debug.Log(coordsStart + "and" + coordsFinish);
                        //запускаем алгоритм поиска пути
                        if (field.array[coordsStart.x, coordsStart.y].getRandomNum() != 0 &&
                           field.array[coordsFinish.x, coordsFinish.y].getRandomNum() != 0)
                        {
                            PathExists = searchPathAStar(coordsStart, coordsFinish);
                        }

                        //если нашли путь выходим из внутреннего цикла
                        if (PathExists == true)
                        {
                            findFlag = true;
                            int idS = field.array[coordsStart.x, coordsStart.y].getId();
                            int idF = field.array[coordsFinish.x, coordsFinish.y].getId();
                            path = (idS, idF);
                            PathFound = false;
                            return 0;
                        }

                    }
                    //если ячейка не может соединиться ни с какой другой того же типа
                    //, то берем следующую
                    //if (coordsCell.y >= field.widthField && coordsCell.x >= field.heightField) return -1;
                    if (coordsCell.y < field.widthField-1)
                        coordsCell.y++;
                    else
                    {
                        if(coordsCell.x < field.heightField-1)
                        {
                            coordsCell.x++;
                            coordsCell.y = 0;
                        }
                        else
                        {
                            coordsCell.x = 0;
                            PathExists = false;
                            break;
                        }
                    }


                }
            }
            
            return 0;
        }

        /*
         * Функция проверяет заблокирована ячейка или нет.
         * В качестве параметра принимает id выбранной ячейки.
         */
        private bool isBlocked(int id)
        {
            //ищем координаты ячейки по id
            var coords = field.findCoordsById(id);
            //переменная подсчета заполненных ячеек вокруг выбранной
            int wallCount = 0;
            //-----------------Процесс проверки-----------------
            //есть ли ячейка вверху
            if (field.array[coords.i + 1, coords.j].getState() == 1) wallCount++;
            //есть ли ячейка внизу
            if (field.array[coords.i - 1, coords.j].getState() == 1) wallCount++;
            //есть ли ячейка слева
            if (field.array[coords.i, coords.j - 1].getState() == 1) wallCount++;
            //есть ли ячейка справа
            if (field.array[coords.i, coords.j + 1].getState() == 1) wallCount++;
            //-----------------Проверка окончена-----------------
            //-----------------Результат проверки----------------
            //если все ячейки вокруг заполнены, то выбранная ячейка заблокирована
            if (wallCount == 4) return true;
            //иначе возвращаем что к выбранной ячейке есть доступ
            return false;
        }

        /*
         * Фукнция ищет id всех ячеек одного типа.
         * В качестве параметра принимает номер типа (randomNum).
         * Возвращаемое значение: список найденных однотипных ячеек.
        */
        private List<(int i, int j)> searchSimilarCells(int type)
        {
            // Переменная хранит в себе список id найденных однотипных ячеек
            List<(int i, int j)> OneTypeCollection = new List<(int i, int j)>();
            /*
             * Начальные координаты цикла (2,2) потому что(0,0) это рамка,
             * а(1, 1) это начальное незаполненное поле для поиска пути.
             */
            for (int i = 0; i < field.heightField; i++)
                for (int j = 0; j < field.widthField; j++)
                {
                    // если мы нашли на карте (в array) ячейку с тем же типом,
                    // что и передали, то записываем в список
                    if (field.array[i, j].getRandomNum() == type)
                    {
                        OneTypeCollection.Add(field.array[i, j].getCoords());
                    }
                        
                }

            //возвращаем собранный список из однотипных элементов
            return OneTypeCollection;
        }

        private bool searchPathAStar((int i, int j) firstClick, (int i, int j) secondClick)
        {
            Point start = new Point(firstClick.i, firstClick.j);
            Point finish = new Point(secondClick.i, secondClick.j);

            SettingsField settings = new SettingsField(field, field.widthField, field.heightField);

            bool flg;
            bool find;
            points = settings.LittlePathfinder(start, finish);
            if (points == null)
                find = false;
            else find = true;

            if (find == false)
            {
                points = settings.LittlePathfinder(start, finish);
                if (points == null)
                    find = false;
                else find = true;

                if (find == false)
                    flg = false;
                else
                    flg = true;

            }
            else
                flg = true;

            if(flg == true)
            {
                return true;
            }
            else
            {
                return false;
            }

            //if (points == null) return false;
            //else return true;
        }

    }
}
