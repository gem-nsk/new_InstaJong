using System;
using System.Collections.Generic;
using genField;

public class RightStrategy : IGameStrategy
{
    public List<int> DoPair((int row, int col) cell1, (int row, int col) cell2, Cell[,] _array)
    {
        var matrix = _array;
        List<int> IDs = new List<int>();

        int firstMoveCol = cell1.col;
        int secondMoveCol = cell2.col;
        if (cell1.row == cell2.row)
        {
            firstMoveCol = cell1.col > cell2.col ? cell2.col : cell1.col;
            secondMoveCol = cell1.col > cell2.col ? cell1.col : cell2.col;
        }
        int cell1Row = cell1.row;
        for (int i = firstMoveCol; i > 0; i--)
        {
            matrix[cell1Row, i].setRandomNum(matrix[cell1Row, i - 1].getRandomNum());
            matrix[cell1Row, i].setState(matrix[cell1Row, i - 1].getState());

            IDs.Add(matrix[cell1Row, i].getId());
        }

        matrix[cell1Row, 0].setState(0);
        matrix[cell1Row, 0].setRandomNum(0);
        IDs.Add(matrix[cell1Row, 0].getId());

        int cell2Row = cell2.row;
        for (int i = secondMoveCol; i > 0; i--)
        {
            matrix[cell2Row, i].setRandomNum(matrix[cell2Row, i - 1].getRandomNum());
            matrix[cell2Row, i].setState(matrix[cell2Row, i - 1].getState());
            IDs.Add(matrix[cell2Row, i].getId());
        }

        matrix[cell2Row, 0].setState(0);
        matrix[cell2Row, 0].setRandomNum(0);
        IDs.Add(matrix[cell2Row, 0].getId());

        return IDs;
    }
}
