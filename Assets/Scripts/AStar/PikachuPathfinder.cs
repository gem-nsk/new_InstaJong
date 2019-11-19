using genField;
using System;

using System.Collections.Generic;
using UnityEngine;

public class PikachuPathfinder : MonoBehaviour
{
    public static int GetCellId(Cell cell, int rowNum, int colNum)
    {
        return colNum * (cell.getCoords().i - 1) + cell.getCoords().j;
    }

    public static int GetCellId(Cell cell, Field field)
    {
        return field.findIdByCoords(cell.getCoords().i, cell.getCoords().j);
    }

    public PikachuPathfinder()
    {
        
    }

    public static Dictionary<int, int> GetAvailablePair(int[,] matrix, Field field)
    {
        Dictionary<int, int> result = new Dictionary<int, int>();
        int row = matrix.GetLength(0);
        int col = matrix.GetLength(1);

        List<int> pairedCellIds = new List<int>();
        float startTime = Time.realtimeSinceStartup;
        for (int i = 1; i < row - 1; i++)
        {
            for (int j = 1; j < col - 1; j++)
            {
                Cell p1 = new Cell(i, j);
                
                if (pairedCellIds.Contains(GetCellId(p1, field)) || matrix[p1.getCoords().i, p1.getCoords().j] == 0)
                    continue;
                for (int x = 1; x < row - 1; x++)
                {
                    for (int y = 1; y < col - 1; y++)
                    {
                        if (x < i && y < j)
                            continue;
                        Cell p2 = new Cell(x, y);
                        if (pairedCellIds.Contains(GetCellId(p2, field)) || matrix[p2.getCoords().i, p2.getCoords().j] == 0)
                            continue;
                        if (GetWayBetweenTwoCell(matrix, p1, p2).Count > 0)
                        {
                            x = i = row;
                            y = j = col;
                            pairedCellIds.Add(GetCellId(p1, field));
                            pairedCellIds.Add(GetCellId(p2, field));
                            x = row - 1;
                            y = col - 1;
                            result.Add(GetCellId(p1, field), GetCellId(p2, field));
                            //Debug.Log("id1: " + p1.getId() + "id2: " + p2.getId());
                            //Debug.LogWarning("match: cell" + p1.getCoords() + " vs " + p2.getCoords());
                        }
                    }
                }
            }
        }
        return result;
    }

    public static int[,] CreateMatrix(Field field)
    {
        
        int row = field.heightField;
        int col = field.widthField;
        int[,] matrix = new int[row, col];
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                matrix[i, j] = field.array[i, j].getRandomNum();
            }
        }

        return matrix;
    }

    public static List<Cell> GetWayBetweenTwoCell(int[,] matrix, Cell cell1, Cell cell2)
    {
        List<Cell> cellsOnLine = new List<Cell>();
        if (matrix[cell1.getCoords().i, cell1.getCoords().j] == 0 ||
            matrix[cell2.getCoords().i, cell2.getCoords().j] == 0)
            return cellsOnLine;

        if(!(cell1.getCoords().i == cell2.getCoords().i && cell1.getCoords().j == cell2.getCoords().j) &&
            matrix[cell1.getCoords().i, cell1.getCoords().j] == matrix[cell2.getCoords().i, cell2.getCoords().j])
        {
            if(cell1.getCoords().i == cell2.getCoords().i)
            {
                //Debug.Log("Line x");
                if(CheckLineX(matrix,cell1.getCoords().j, cell2.getCoords().j, cell1.getCoords().i))
                {
                    //Debug.Log("ok line x");
                    cellsOnLine.Add(cell1);
                    cellsOnLine.Add(cell2);
                    return cellsOnLine;
                }
            }
            if (cell1.getCoords().j == cell2.getCoords().j)
            {
                //Debug.Log("Line y");
                if (CheckLineY(matrix, cell1.getCoords().i, cell2.getCoords().i, cell1.getCoords().j))
                {
                    //Debug.Log("ok line y");
                    cellsOnLine.Add(cell1);
                    cellsOnLine.Add(cell2);
                    return cellsOnLine;
                }
            }

            int t = -1;

            // check in rectangle with x
            if ((t = CheckRectX(matrix, cell1, cell2)) != -1)
            {
                //Debug.Log("rect x");
                cellsOnLine.Add(cell1);
                cellsOnLine.Add(new Cell(cell1.getCoords().i, t));
                cellsOnLine.Add(new Cell(cell2.getCoords().i, t));
                cellsOnLine.Add(cell2);
                return cellsOnLine;
            }
            // check in rectangle with y
            if ((t = CheckRectY(matrix, cell1, cell2)) != -1)
            {
                //Debug.Log("rect y");
                cellsOnLine.Add(cell1);
                cellsOnLine.Add(new Cell(t, cell1.getCoords().j));
                cellsOnLine.Add(new Cell(t, cell2.getCoords().j));
                cellsOnLine.Add(cell2);

                return cellsOnLine;
            }
            // check more right
            if ((t = CheckMoreLineX(matrix, cell1, cell2, 1)) != -1)
            {
                //Debug.Log("more right");
                cellsOnLine.Add(cell1);
                cellsOnLine.Add(new Cell(cell1.getCoords().i, t));
                cellsOnLine.Add(new Cell(cell2.getCoords().i, t));
                cellsOnLine.Add(cell2);

                return cellsOnLine;
                // return new MyLine(new Point(p1.x, t), new Point(p2.x, t));
            }
            // check more left
            if ((t = CheckMoreLineX(matrix, cell1, cell2, -1)) != -1)
            {
                //Debug.Log("more left");
                cellsOnLine.Add(cell1);
                cellsOnLine.Add(new Cell(cell1.getCoords().i, t));
                cellsOnLine.Add(new Cell(cell2.getCoords().i, t));
                cellsOnLine.Add(cell2);

                return cellsOnLine;
                // return new MyLine(new Point(p1.x, t), new Point(p2.x, t));
            }
            // check more down
            if ((t = CheckMoreLineY(matrix, cell1, cell2, 1)) != -1)
            {
                //Debug.Log("more down");
                cellsOnLine.Add(cell1);
                cellsOnLine.Add(new Cell(t, cell1.getCoords().j));
                cellsOnLine.Add(new Cell(t, cell2.getCoords().j));
                cellsOnLine.Add(cell2);

                return cellsOnLine;
                // return new MyLine(new Point(t, p1.y), new Point(t, p2.y));
            }
            // check more up
            if ((t = CheckMoreLineY(matrix, cell1, cell2, -1)) != -1)
            {
                //Debug.Log("more up");
                cellsOnLine.Add(cell1);
                cellsOnLine.Add(new Cell(t, cell1.getCoords().j));
                cellsOnLine.Add(new Cell(t, cell2.getCoords().j));
                cellsOnLine.Add(cell2);

                return cellsOnLine;
                // return new MyLine(new Point(t, p1.y), new Point(t, p2.y));
            }
        }
        return cellsOnLine;
    }

    private static bool CheckLineY(int[,] matrix, int x1, int x2, int y)
    {
        int min = Math.Min(x1, x2);
        int max = Math.Max(x1, x2);

        for (int x = min + 1; x < max; x++)
        {
            if (matrix[x, y] > 0)
            {
                return false;
            }
        }
        return true;
    }

    private static bool CheckLineX(int[,] matrix, int y1, int y2, int x)
    {
        int min = Math.Min(y1, y2);
        int max = Math.Max(y1, y2);

        for(int y = min + 1; y < max; y++)
        {
            if(matrix[x,y] > 0)
            {
                return false;
            }
        }
        return true;
    }

    private static int CheckRectX(int[,] matrix, Cell cell1, Cell cell2)
    {
        // find point have y min and max
        Cell minColCell = cell1, maxColCell = cell2;
        if (cell1.getCoords().j > cell2.getCoords().j)
        {
            minColCell = cell2;
            maxColCell = cell1;
        }
        for (int y = minColCell.getCoords().j; y <= maxColCell.getCoords().j; y++)
        {
            if (y > minColCell.getCoords().j && matrix[minColCell.getCoords().i, y] > 0)
            {
                return -1;
            }
            // check two line
            if ((matrix[maxColCell.getCoords().i, y] == 0 || y == maxColCell.getCoords().j)
                    && CheckLineY(matrix, minColCell.getCoords().i, maxColCell.getCoords().i, y)
                    && CheckLineX(matrix, y, maxColCell.getCoords().j, maxColCell.getCoords().i))
            {

                //Debug.Log("Rect x");
                //Debug.Log("(" + minColCell.getCoords().i + "," + minColCell.getCoords().j + ") -> ("
                //        + minColCell.getCoords().i + "," + y + ") -> (" + maxColCell.getCoords().i + "," + y
                //        + ") -> (" + maxColCell.getCoords().i + "," + maxColCell.getCoords().j + ")");
                // if three line is true return column y
                return y;
            }
        }
        // have a line in three line not true then return -1
        return -1;
    }

    private static int CheckRectY(int[,] matrix, Cell cell1, Cell cell2)
    {
        //Debug.Log("check rect y");
        // find point have y min
        Cell minRowCell = cell1, maxRowCell = cell2;
        if (cell1.getCoords().i > cell2.getCoords().i)
        {
            minRowCell = cell2;
            maxRowCell = cell1;
        }
        // find line and y begin
        for (int x = minRowCell.getCoords().i; x <= maxRowCell.getCoords().i; x++)
        {
            if (x > minRowCell.getCoords().i && matrix[x, minRowCell.getCoords().j] > 0)
            {
                return -1;
            }
            if ((matrix[x, maxRowCell.getCoords().j] == 0 || x == maxRowCell.getCoords().i)
                    && CheckLineX(matrix, minRowCell.getCoords().j, maxRowCell.getCoords().j, x)
                    && CheckLineY(matrix, x, maxRowCell.getCoords().i, maxRowCell.getCoords().j))
            {

                //Debug.Log("Rect y");
                //Debug.Log("(" + minRowCell.getCoords().i + "," + minRowCell.getCoords().j + ") -> (" + x
                //        + "," + minRowCell.getCoords().j + ") -> (" + x + "," + maxRowCell.getCoords().j
                //        + ") -> (" + maxRowCell.getCoords().i + "," + maxRowCell.getCoords().j + ")");
                return x;
            }
        }
        return -1;
    }

    /**
	 * p1 and p2 are Points want check
	 * 
	 * @param type
	 *            : true is check with increase, false is decrease return column
	 *            can connect p1 and p2
	 */
    private static int CheckMoreLineX(int[,] matrix, Cell cell1, Cell cell2, int type)
    {
        //Debug.Log("check chec more x");
        // find point have y min
        Cell minColCell = cell1, maxColCell = cell2;
        if (cell1.getCoords().j > cell2.getCoords().j)
        {
            minColCell = cell2;
            maxColCell = cell1;
        }
        // find line and y begin
        int y = maxColCell.getCoords().j + type;
        int row = minColCell.getCoords().i;
        int colFinish = maxColCell.getCoords().j;
        if (type == -1)
        {
            colFinish = minColCell.getCoords().j;
            y = minColCell.getCoords().j + type;
            row = maxColCell.getCoords().i;
            //Debug.Log("colFinish = " + colFinish);
        }

        // find column finish of line

        // check more
        if ((matrix[row, colFinish] == 0 || minColCell.getCoords().j == maxColCell.getCoords().j)
                && CheckLineX(matrix, minColCell.getCoords().j, maxColCell.getCoords().j, row))
        {
            while (matrix[minColCell.getCoords().i, y] == 0
                    && matrix[maxColCell.getCoords().i, y] == 0)
            {
                if (CheckLineY(matrix, minColCell.getCoords().i, maxColCell.getCoords().i, y))
                {

                    //Debug.Log("TH X " + type);
                    //Debug.Log("(" + minColCell.getCoords().i + "," + minColCell.getCoords().i + ") -> ("
                    //        + minColCell.getCoords().i + "," + y + ") -> (" + maxColCell.getCoords().i + "," + y
                    //        + ") -> (" + maxColCell.getCoords().i + "," + maxColCell.getCoords().i + ")");
                    return y;
                }
                y += type;
            }
        }
        return -1;
    }

    private static int CheckMoreLineY(int[,] matrix, Cell cell1, Cell cell2, int type)
    {
        //Debug.Log("check more y");
        Cell minRowCell = cell1, maxRowCell = cell2;
        if (cell1.getCoords().i > cell2.getCoords().i)
        {
            minRowCell = cell2;
            maxRowCell = cell1;
        }
        int x = maxRowCell.getCoords().i + type;
        int col = minRowCell.getCoords().j;
        int rowFinish = maxRowCell.getCoords().i;
        if (type == -1)
        {
            rowFinish = minRowCell.getCoords().i;
            x = minRowCell.getCoords().i + type;
            col = maxRowCell.getCoords().j;
        }
        if ((matrix[rowFinish, col] == 0 || minRowCell.getCoords().i == maxRowCell.getCoords().i)
                && CheckLineY(matrix, minRowCell.getCoords().i, maxRowCell.getCoords().i, col))
        {
            while (matrix[x, minRowCell.getCoords().j] == 0
                    && matrix[x, maxRowCell.getCoords().j] == 0)
            {
                if (CheckLineX(matrix, minRowCell.getCoords().j, maxRowCell.getCoords().j, x))
                {
                    //Debug.Log("TH Y " + type);
                    //Debug.Log("(" + minRowCell.getCoords().i + "," + minRowCell.getCoords().j + ") -> ("
                    //        + x + "," + minRowCell.getCoords().j + ") -> (" + x + "," + maxRowCell.getCoords().j
                    //        + ") -> (" + maxRowCell.getCoords().i + "," + maxRowCell.getCoords().j + ")");
                    return x;
                }
                x += type;
            }
        }
        return -1;
    }
}
