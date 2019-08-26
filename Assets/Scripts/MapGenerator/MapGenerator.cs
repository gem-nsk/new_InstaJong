using System;
using System.IO;

namespace genField
{
    class MapGenerator
    {
        private int countElem = 0;
        private int countImagesInType = 0;
        private int countTypes = 0;
        public Field field;

        public MapGenerator() { }

        public Field mapFromString(String[,] map, int width, int height)
        {
            

            for(int i = 0; i < height; i++)
                for(int j = 0; j < width; j++)
                    if(map[i,j] == "1")
                        countElem++;

            //Console.WriteLine(countElem);

            if (countElem % 4 == 0) { countTypes = countElem / 4; countImagesInType = 4; }
            else if(countElem % 2 == 0) { countTypes = countElem / 2; countImagesInType = 2; }
            //else { return -1; }


            field = new Field(width, height, countTypes, countImagesInType);
            field.initField(false);

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if (map[i, j] == "1")
                        field.array[i, j].setState(1);

            field.generateField();
            

            return field;
        }

        public (String[,] map, int width, int height) mapFromFile(String filename)
        {

            int width = 0;
            int height = 0;
            string[] lines = File.ReadAllLines(filename);
            String[,] map = new String[lines.Length, lines[0].Split(' ').Length];
            for (int i = 0; i < lines.Length; i++)
            {
                
                string[] temp = lines[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    map[i, j] = temp[j];
                height = lines.Length;
                width = temp.Length;
            }
            //mapFromString(map, width, height);
            return (map,width,height);
        }
    }
}
