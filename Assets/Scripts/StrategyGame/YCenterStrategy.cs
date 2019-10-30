using System;
using System.Collections.Generic;
using genField;

public class YCenterStrategy : IGameStrategy
{
    public List<int> DoPair((int row, int col) cell1, (int row, int col) cell2, Cell[,] _array)
    {
        var matrix = _array;
        List<int> IDs = new List<int>();

        matrix[cell1.row, cell1.col].setState(0);
        matrix[cell1.row, cell1.col].setRandomNum(0);

        matrix[cell2.row, cell2.col].setState(0);
        matrix[cell2.row, cell2.col].setRandomNum(0);

        IDs.Add(matrix[cell1.row, cell1.col].getId());
        IDs.Add(matrix[cell2.row, cell2.col].getId());

        int rowNum = matrix.GetLength(0);
        int colNum = matrix.GetLength(1);

        int firstMoveRow = cell1.row;
        int secondMoveRow = cell2.row;
        if (cell1.col == cell2.col && cell1.row <= (rowNum - 2) / 2 && cell2.row <= (rowNum - 2) / 2)
        {
            firstMoveRow = cell1.row > cell2.row ? cell2.row : cell1.row;
            secondMoveRow = cell1.row > cell2.row ? cell1.row : cell2.row;
        }

        if (cell1.col == cell2.col && cell1.row > (rowNum - 2) / 2 && cell2.row > (rowNum - 2) / 2)
        {
            firstMoveRow = cell1.row > cell2.row ? cell1.row : cell2.row;
            secondMoveRow = cell1.row > cell2.row ? cell2.row : cell1.row;
        }

        int cell1Col = cell1.col;
        if (cell1.row <= (rowNum - 2) / 2)
        {
            for (int i = firstMoveRow; i > 0; i--)
            {
                matrix[i, cell1Col].setRandomNum(matrix[i - 1, cell1Col].getRandomNum());
                matrix[i, cell1Col].setState(matrix[i - 1, cell1Col].getState());

                IDs.Add(matrix[i, cell1Col].getId());
            }
        }
        else
        {
            for (int i = firstMoveRow; i < (rowNum - 2); i++)
            {
                matrix[i, cell1Col].setRandomNum(matrix[i + 1, cell1Col].getRandomNum());
                matrix[i, cell1Col].setState(matrix[i + 1, cell1Col].getState());

                IDs.Add(matrix[i, cell1Col].getId());
            }
            matrix[(rowNum - 2), cell1.col].setState(0);
            matrix[(rowNum - 2), cell1.col].setRandomNum(0);
            IDs.Add(matrix[(rowNum - 2), cell1.col].getId());

        }

        int cell2Col = cell2.col;
        if (cell2.row <= (rowNum - 2) / 2)
        {
            for (int i = secondMoveRow; i > 0; i--)
            {
                matrix[i, cell2Col].setRandomNum(matrix[i - 1, cell2Col].getRandomNum());
                matrix[i, cell2Col].setState(matrix[i - 1, cell2Col].getState());

                IDs.Add(matrix[i, cell2Col].getId());
            }
        }
        else
        {
            for (int i = secondMoveRow; i < (rowNum - 2); i++)
            {
                matrix[i, cell2Col].setRandomNum(matrix[i + 1, cell2Col].getRandomNum());
                matrix[i, cell2Col].setState(matrix[i + 1, cell2Col].getState());

                IDs.Add(matrix[i, cell2Col].getId());
            }
            matrix[(rowNum - 2) , cell2.col].setState(0);
            matrix[(rowNum - 2), cell2.col].setRandomNum(0);
            IDs.Add(matrix[(rowNum - 2), cell2.col].getId());

        }

        return IDs;
    }
}
