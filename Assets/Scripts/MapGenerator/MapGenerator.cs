using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace genField
{
    public class MapGenerator
    {
        private int countElem = 0;
        private int countImagesInType = 0;
        private int countTypes = 0;
        public Field field;

        public MapGenerator() { }

        public Field mapFromString(String[,] map, int width, int height)
        {


            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    if (map[i, j] == "1")
                        countElem++;

            //Console.WriteLine(countElem);

            if (countElem % 4 == 0) { countTypes = countElem / 4; countImagesInType = 4; }
            else if (countElem % 2 == 0) { countTypes = countElem / 2; countImagesInType = 2; }
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

            // UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filename);

            /*
            List<string> lines = new List<string>();

          

            //StreamReader reader = new StreamReader(filename);

            string _line;

            while ((_line = reader.ReadLine()) != null)
            {
                lines.Add(_line);
                _line = "";
            }

            foreach (string line in lines)
            {
                Debug.Log(line);

            }*/
            
            string[] lines = filename.Split('\n');
            //string[] lines = File.ReadAllLines("C:\\Users\\Gem\\AppData\\Local\\Temp\\GeM\\InstaJong\\States.txt");
            String[,] map = new String[lines.Length, lines[0].Split(' ').Length];

            for (int i = 0; i < lines.Length-1; i++)
            {
                string[] temp = lines[i].Split(' ');
                for (int j = 0; j < temp.Length; j++)
                    map[i, j] = temp[j];
                height = lines.Length;
                width = temp.Length;
            }
            Debug.Log("Height: " + height);
            Debug.Log("Width: " + width);
            //mapFromString(map, width, height);
            return (map, width, height);
        }

    }
}
