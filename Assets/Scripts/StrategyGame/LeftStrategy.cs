using System;
using System.Collections.Generic;
using genField;

public class LeftStrategy : IGameStrategy
{
    public List<int> DoPair((int row, int col) cell1, (int row, int col) cell2, Cell[,] _array)
    {
        var matrix = _array;
        List<int> IDs = new List<int>();

        int firstMoveCol = cell1.col;
        int secondMoveCol = cell2.col;
        if (cell1.row == cell2.row)
        {
            firstMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
            secondMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
        }
        int cell1Row = cell1.row;
        for (int i = firstMoveCol; i < matrix.GetLength(1) - 2; i++)
        {
            matrix[cell1Row, i].setRandomNum(matrix[cell1Row, i + 1].getRandomNum());
            matrix[cell1Row, i].setState(matrix[cell1Row, i + 1].getState());

            IDs.Add(matrix[cell1Row, i].getId());
        }

        matrix[cell1Row, matrix.GetLength(1) - 2].setState(0);
        matrix[cell1Row, matrix.GetLength(1) - 2].setRandomNum(0);
        IDs.Add(matrix[cell1Row, matrix.GetLength(1) - 2].getId());

        int cell2Row = cell2.row;
        for (int i = secondMoveCol; i < matrix.GetLength(0) - 2; i++)
        {
            matrix[cell2Row, i].setRandomNum(matrix[cell1Row, i + 1].getRandomNum());
            matrix[cell2Row, i].setState(matrix[cell1Row, i + 1].getState());
            IDs.Add(matrix[i, cell2Row].getId());
        }

        matrix[cell2Row, matrix.GetLength(1) - 2].setState(0);
        matrix[cell2Row, matrix.GetLength(1) - 2].setRandomNum(0);
        IDs.Add(matrix[cell2Row, matrix.GetLength(1) - 2].getId());

        return IDs;
    }
}
