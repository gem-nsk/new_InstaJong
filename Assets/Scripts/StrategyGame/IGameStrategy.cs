using System.Collections.Generic;
using genField;

public interface IGameStrategy
{
    List<int> DoPair((int row, int col) cell1, (int row, int col) cell2, Cell[,] _array);
}