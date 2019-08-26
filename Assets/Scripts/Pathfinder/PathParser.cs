using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genField
{
    public class PathParser
    {
        private Move move;
        private Field field;
        
        // флаг, кторый обозначает найден ли пользователем сохраненный путь
        public bool PathFound { get; set; }
        // флаг, который обозначает существует еще ход на карте или нет
        public bool PathExists { get; set; }

        public (int idFirst, int idSecond) path;

        //Конструктор класса
        public PathParser(Field field)
        {
            /*
             * Инициализируем подключаемы модули: 
             *  - генерация карты
             *  - волновой метод поиска пути
             */
            move = new Move();
            this.field = field;
            PathFound = false;
            PathExists = false;
        }

        //функция поиска возможного хода
        public int parse()
        {
            
            //проверка флагов, чтобы не искали заново
            //if (PathFound == false) return 0;
            if (PathExists == true) return 0;

            //флаг для обозначения нашли возможный ход или нет
            bool findFlag = false;
            /*
             * coordsCell - переменная, которая хранит координаты проверяемой ячейки.
             * Начальные координаты (2,2) потому что (0,0) это рамка,
             * а (1,1) это начальное незаполненное поле для поиска пути.
             */
            (int x, int y) coordsCell = (2,2);

            //ищем, пока не будет найден возможный ход
            while(findFlag == false)
            {
                //берем id и randomNum 
                
                int id = field.array[coordsCell.x, coordsCell.y].getId();
                int randomNum = field.array[coordsCell.x, coordsCell.y].getRandomNum();

                //ищем все ячейки с одинаковым типом и записываем в список
                List<int> foundIdCells = searchSimilarCells(randomNum);
                    
                /*
                 * Процесс поиска происходит следующим образом
                 * перебором среди списка однотипных ячеек ищем пути
                 * если путь есть, то сохраняем этот путь и завершаем поиск
                 * если нет, то берем следующий тип и проводим те же операции
                 */
                if(PathExists == false)
                {
                    for (int i = 0; i < foundIdCells.Count-1; i++)
                    {
                        //проверяем, не заблокированы ли ячейки, если хоть одна заблокирована,
                        // смысла проводить вычисления нет
                        if (isBlocked(foundIdCells[i]) || isBlocked(foundIdCells[i+1])) continue;
                        //берем координаты первой и второй ячейки
                        (int x, int y) coordsStart = field.findCoordsById(foundIdCells[i]);
                        (int x, int y) coordsFinish = field.findCoordsById(foundIdCells[i+1]);

                        //запускаем алгоритм поиска пути
                        PathExists = move.FindWave(
                            coordsStart.y, coordsStart.x,
                            coordsFinish.y, coordsFinish.x,
                            field.array);

                        //если нашли путь выходим из внутреннего цикла
                        if (PathExists == true)
                        {
                            findFlag = true;
                            path = (foundIdCells[i], foundIdCells[i + 1]);
                            break;
                        }

                    }
                    //если ячейка не может соединиться ни с какой другой того же типа
                    //, то берем следующую
                    if (coordsCell.y < field.widthField-1)
                       coordsCell.y++;
                    else
                    {
                        coordsCell.x++;
                        coordsCell.y = 2;
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
        private List<int> searchSimilarCells(int type)
        {
            // Переменная хранит в себе список id найденных однотипных ячеек
            List<int> OneTypeCollection = new List<int>();
            /*
             * Начальные координаты цикла (2,2) потому что(0,0) это рамка,
             * а(1, 1) это начальное незаполненное поле для поиска пути.
             */
            for (int i = 2; i < field.heightField-2; i++)
                for(int j = 2; j < field.widthField-2; j++)
                {
                    // если мы нашли на карте (в array) ячейку с тем же типом,
                    // что и передали, то записываем в список
                    if (field.array[i, j].getRandomNum() == type)
                        OneTypeCollection.Add(field.array[i, j].getId());
                }

            //возвращаем собранный список из однотипных элементов
            return OneTypeCollection;
        }
    }
}
