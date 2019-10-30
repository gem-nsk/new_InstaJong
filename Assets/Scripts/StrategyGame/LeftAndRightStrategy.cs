using System;
using System.Collections.Generic;
using genField;

public class LeftAndRightStrategy : IGameStrategy
{
    public List<int> DoPair((int row, int col) cell1, (int row, int col) cell2, Cell[,] _array)
    {
        var matrix = _array;
        List<int> IDs = new List<int>();

        int firstMoveCol = cell1.col;
        int secondMoveCol = cell2.col;
        int rowNum = matrix.GetLength(0);
        int colNum = matrix.GetLength(1);

        if (cell1.row == cell2.row && cell1.col < (colNum - 2) / 2 && cell2.col < (colNum - 2) / 2)
        {
            firstMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
            secondMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
        }

        if (cell1.row == cell2.row && cell1.col > (colNum - 2) / 2 && cell2.col > (colNum - 2) / 2)
        {
            firstMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
            secondMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
        }
        //for first cell
        int cell1Row = cell1.row;
        if (cell1.col <= (colNum - 2) / 2)
        {
            for (int i = firstMoveCol; i < (colNum - 2) / 2; i++)
            {

                matrix[cell1Row, i].setRandomNum(matrix[cell1Row, i + 1].getRandomNum());
                matrix[cell1Row, i].setState(matrix[cell1Row, i + 1].getState());

                IDs.Add(matrix[cell1Row, i].getId());

            }

            matrix[cell1Row, (colNum - 2) / 2].setState(0);
            matrix[cell1Row, (colNum - 2) / 2].setRandomNum(0);
            IDs.Add(matrix[cell1Row, (colNum - 2) / 2].getId());

        }
        else
        {
            for (int i = firstMoveCol; i > (colNum - 2) / 2; i--)
            {
                matrix[cell1Row, i].setRandomNum(matrix[cell1Row, i - 1].getRandomNum());
                matrix[cell1Row, i].setState(matrix[cell1Row, i - 1].getState());

                IDs.Add(matrix[cell1Row, i].getId());
            }

            matrix[cell1Row, (colNum - 2) / 2 + 1].setState(0);
            matrix[cell1Row, (colNum - 2) / 2 + 1].setRandomNum(0);
            IDs.Add(matrix[cell1Row, (colNum - 2) / 2 + 1].getId());

        }
        //for second cell
        int cell2Row = cell2.row;
        if (cell2.col <= (colNum - 2) / 2)
        {
            for (int i = secondMoveCol; i < (colNum - 2) / 2; i++)
            {
                matrix[cell2Row, i].setRandomNum(matrix[cell2Row, i + 1].getRandomNum());
                matrix[cell2Row, i].setState(matrix[cell2Row, i + 1].getState());

                IDs.Add(matrix[cell2Row, i].getId());
            }

            matrix[cell2Row, (colNum - 2) / 2].setState(0);
            matrix[cell2Row, (colNum - 2) / 2].setRandomNum(0);
            IDs.Add(matrix[cell2Row, (colNum - 2) / 2].getId());
        }
        else
        {
            for (int i = secondMoveCol; i > (colNum - 2) / 2 + 1; i--)
            {
                matrix[cell2Row, i].setRandomNum(matrix[cell2Row, i - 1].getRandomNum());
                matrix[cell2Row, i].setState(matrix[cell2Row, i - 1].getState());

                IDs.Add(matrix[cell1Row, i].getId());
            }
            matrix[cell2Row, (colNum - 2) / 2 + 1].setState(0);
            matrix[cell2Row, (colNum - 2) / 2 + 1].setRandomNum(0);
            IDs.Add(matrix[cell2Row, (colNum - 2) / 2 + 1].getId());
        }

        return IDs;
    }
}
