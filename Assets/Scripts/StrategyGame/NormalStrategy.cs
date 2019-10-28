using System.Collections.Generic;
using genField;

public class NormalStrategy : IGameStrategy
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

        return IDs;
    }
}
