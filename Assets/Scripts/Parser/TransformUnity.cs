using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genField
{
    public class TransformUnity
    {

        public TransformUnity() { }
        //функция заполняющая массив из файлов
        public Field fromFileToUnity ()
        {
            MapGenerator mapGenerator = new MapGenerator();

            var mapStates = mapGenerator.mapFromFile("States.txt");
            var mapRandomNums = mapGenerator.mapFromFile("RandomNums.txt");
            var mapIDs = mapGenerator.mapFromFile("IDs.txt");

            int width = mapStates.width;
            int height = mapStates.height;
            
            Field field = new Field(mapStates.width,mapStates.height);
            
            field.initField(true);

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    field.array[i, j].setState(Convert.ToInt32(mapStates.map[i,j]));

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    field.array[i, j].setRandomNum(Convert.ToInt32(mapRandomNums.map[i, j]));

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    field.array[i, j].setId(Convert.ToInt32(mapIDs.map[i, j]));

            return field;

        }
        //функция сохраняющая массив в файлы
        public void fromUnityToFile(Field field)
        {
            
            using (StreamWriter sw = new StreamWriter("States.txt"))
            {
                for (int i = 0; i < field.heightField; i++)
                {
                    for (int j = 0; j < field.widthField; j++)
                    {
                        if(j < field.widthField-1)
                            sw.Write(field.array[i, j].getState() + " ");
                        else
                            sw.Write(field.array[i, j].getState());
                    }
                    sw.WriteLine();

                }
            }

            using (StreamWriter sw = new StreamWriter("RandomNums.txt"))
            {
                for (int i = 0; i < field.heightField; i++)
                {
                    for (int j = 0; j < field.widthField; j++)
                    {
                        if (j < field.widthField - 1)
                            sw.Write(field.array[i, j].getRandomNum() + " ");
                        else
                            sw.Write(field.array[i, j].getRandomNum());
                    }
                    sw.WriteLine();
                }
            }

            using (StreamWriter sw = new StreamWriter("IDs.txt"))
            {
                for (int i = 0; i < field.heightField; i++)
                {
                    for (int j = 0; j < field.widthField; j++)
                    {
                        if (j < field.widthField - 1)
                            sw.Write(field.array[i, j].getId() + " ");
                        else
                            sw.Write(field.array[i, j].getId());
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}
