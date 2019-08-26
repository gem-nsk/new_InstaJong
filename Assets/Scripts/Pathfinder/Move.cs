using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genField
{
    class Move
    {
        int MapWidht = 20;
        int MapHeight = 13;
        int turn1;
        int turn2;
        int turn3;
        int turn4;
        int turn5;
        int turn6;
        int turn7;
        int turn8;

        public Move() { }
        
        public bool FindWave(int startX, int startY, int targetX, int targetY, Cell[,] map)
        {
            bool add = true;
            int[,] cMap = new int[MapHeight, MapWidht];
            int x, y, step = 0;
            for (y = 0; y < MapHeight; y++)
                for (x = 0; x < MapWidht; x++)
                {
                    if (map[y, x].getState() == 2)
                    {
                        cMap[y, x] = -2;
                    }
                    else if(map[y, x].getState() == 1)
                    {
                        cMap[y, x] = -3;
                    }
                    else
                    {
                        cMap[y, x] = -1;
                    }
                }
            cMap[startY, startX] = -1;
            cMap[targetY, targetX] = 0;
            while (add == true)
            {
                add = false;
                for (y = 0; y < MapWidht; y++)
                    for (x = 0; x < MapHeight; x++)
                    {
                        if (cMap[x, y] == step)
                        {
                            if ((startY < targetY && startX == targetX) || startY < targetY) // ↑
                            {
                                if (y - 1 >= 0 && cMap[x - 1, y] != -2 && cMap[x - 1, y] == -1)
                                {
                                    cMap[x - 1, y] = step + 1;
                                
                                }
                                if (x - 1 >= 0 && cMap[x, y - 1] != -2 && cMap[x, y - 1] == -1)
                                {
                                    cMap[x, y - 1] = step + 1;
                                    turn1 = 1;
                                }
                                //if (y + 1 < MapWidht && cMap[x + 1, y] != -2 && cMap[x + 1, y] == -1)
                                //{
                                //    cMap[x + 1, y] = step + 1;
                                //}
                                if (x + 1 < MapHeight && cMap[x, y + 1] != -2 && cMap[x, y + 1] == -1)
                                {
                                    cMap[x, y + 1] = step + 1;
                                    turn2 = 1;
                                }

                                if (startY < targetY && turn1 == 1)
                                {
                                    if (y + 1 < MapWidht && cMap[x + 1, y] != -2 && cMap[x + 1, y] == -1)
                                    {
                                        cMap[x + 1, y] = step + 1;
                                        turn3 = 1;
                                        turn2 = 0;
                                    }
                                }

                            }

                            if ((startY > targetY && startX == targetX) || startY > targetY) // ↓
                            {
                                //if (y - 1 >= 0 && cMap[x - 1, y] != -2 && cMap[x - 1, y] == -1)
                                //{
                                //    cMap[x - 1, y] = step + 1;
                                //}
                                if (x - 1 >= 0 && cMap[x, y - 1] != -2 && cMap[x, y - 1] == -1)
                                {
                                    cMap[x, y - 1] = step + 1;
                                    turn3 = 1;
                                }
                                if (y + 1 < MapWidht && cMap[x + 1, y] != -2 && cMap[x + 1, y] == -1)
                                {
                                    cMap[x + 1, y] = step + 1;
                                }
                                if (x + 1 < MapHeight && cMap[x, y + 1] != -2 && cMap[x, y + 1] == -1)
                                {
                                    cMap[x, y + 1] = step + 1;
                                    turn4 = 1;
                                }

                                if (startX < targetX && turn3 == 1)
                                {
                                    if (y + 1 < MapWidht && cMap[x - 1, y] != -2 && cMap[x - 1, y] == -1)
                                    {
                                        cMap[x - 1, y] = step + 1;
                                        turn8 = 1;
                                        turn4 = 0;
                                    }
                                }
                            }

                            if (startX > targetX && startY == targetY) // →
                            {
                                if (y - 1 >= 0 && cMap[x - 1, y] != -2 && cMap[x - 1, y] == -1)
                                {
                                    cMap[x - 1, y] = step + 1;
                                    turn5 = 1;
                                }
                                //if (x - 1 >= 0 && cMap[x, y - 1] != -2 && cMap[x, y - 1] == -1)
                                //{
                                //    cMap[x, y - 1] = step + 1;
                                //}
                                if (y + 1 < MapWidht && cMap[x + 1, y] != -2 && cMap[x + 1, y] == -1)
                                {
                                    cMap[x + 1, y] = step + 1;
                                    turn6 = 1;
                                }
                                if (x + 1 < MapHeight && cMap[x, y + 1] != -2 && cMap[x, y + 1] == -1)
                                {
                                    cMap[x, y + 1] = step + 1;
                                }
                            }

                            if (startX < targetX && startY == targetY) // ←
                            {
                                if (y - 1 >= 0 && cMap[x - 1, y] != -2 && cMap[x - 1, y] == -1)
                                {
                                    cMap[x - 1, y] = step + 1;
                                    turn7 = 1;
                                }
                                if (x - 1 >= 0 && cMap[x, y - 1] != -2 && cMap[x, y - 1] == -1)
                                {
                                    cMap[x, y - 1] = step + 1;
                                }
                                if (y + 1 < MapWidht && cMap[x + 1, y] != -2 && cMap[x + 1, y] == -1)
                                {
                                    cMap[x + 1, y] = step + 1;
                                    turn8 = 1;
                                }
                                //if (x + 1 < MapHeight && cMap[x, y + 1] != -2 && cMap[x, y + 1] == -1)
                                //{
                                //    cMap[x, y + 1] = step + 1;
                                //}
                            }
                        }
                    }
                step++;
                add = true;
                if (cMap[startY, startX] != -1)
                {
                    add = false;
                    return true;
                    //Console.Write(" Yessssssss");
                }

                if ((turn1 + turn2 + turn3 + turn4 + turn5 + turn6 + turn7 + turn8) > 2)
                {
                    add = false;
                    return false;
                    //Console.Write(" NOOOOOOOO");
                }

                if (step > MapWidht * MapHeight)
                {
                    add = false;
                    return false;
                    //Console.Write(" NOOOOOOOO");
                }
            }

            //for (y = 0; y < MapHeight; y++)
            //{
            //    Console.WriteLine();
            //    for (x = 0; x < MapWidht; x++)
            //        if (cMap[y, x] == -1)
            //        {
            //            Console.Write(" ");
            //        }
            //        else if (cMap[y, x] == -3)
            //        {
            //            Console.Write("*");
            //        }
            //        else if (cMap[y, x] == -2)
            //        {
            //            Console.Write("#");
            //        }
            //        else if (y == startY && x == startX)
            //        {
            //            Console.Write("S");
            //        }
            //        else if (y == targetY && x == targetX)
            //        {
            //            Console.Write("F");
            //        }
            //        else if (cMap[y, x] > -1)
            //        {
            //            Console.Write("{0}", cMap[y, x]);
            //        }
            //}
            //Console.ReadKey();
            return false;
        }
    }
}
