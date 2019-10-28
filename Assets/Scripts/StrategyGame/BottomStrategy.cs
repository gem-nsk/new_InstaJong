using System.Collections.Generic;
using genField;

public class BottomStrategy : IGameStrategy
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


        int firstMoveRow = cell1.row;
        int secondMoveRow = cell2.row;
        if (cell1.col == cell2.col)
        {
            firstMoveRow = cell1.row > cell2.row ? cell1.row : cell2.row;
            secondMoveRow = cell1.row > cell2.row ? cell2.row : cell1.row;
        }
        int cell1Col = cell1.col;
        for (int i = firstMoveRow; i < matrix.GetLength(0) - 2; i++)
        {
            matrix[i, cell1Col].setRandomNum(matrix[i + 1, cell1Col].getRandomNum());
            matrix[i, cell1Col].setState(matrix[i + 1, cell1Col].getState());

            IDs.Add(matrix[i, cell1Col].getId());
        }

        matrix[matrix.GetLength(0) - 2, cell1.col].setState(0);
        matrix[matrix.GetLength(0) - 2, cell1.col].setRandomNum(0);
        IDs.Add(matrix[matrix.GetLength(0) - 2, cell1.col].getId());

        int cell2Col = cell2.col;
        for (int i = secondMoveRow; i < matrix.GetLength(0) - 2; i++)
        {
            matrix[i, cell2Col].setRandomNum(matrix[i + 1, cell2Col].getRandomNum());
            matrix[i, cell2Col].setState(matrix[i + 1, cell2Col].getState());
            IDs.Add(matrix[i, cell2Col].getId());
        }

        matrix[matrix.GetLength(0) - 2, cell2.col].setState(0);
        matrix[matrix.GetLength(0) - 2, cell2.col].setRandomNum(0);
        IDs.Add(matrix[matrix.GetLength(0) - 2, cell1Col].getId());


        return IDs;
    }
}