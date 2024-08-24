using System;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{
    public int RowCount;
    public int ColumnCount;

    private int[,] _grid;
    public Matrix(int row, int col, int val)
    {
        RowCount = row;
        ColumnCount = col;

        _grid = new int[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                _grid[i, j] = val;
            }
        }
    }
    public bool IsRow(int row, int col, int length)
    {
        if (row < 0 || row > RowCount || col < 0 || col > ColumnCount)
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        int currentElement = _grid[row, col];
        int consecutiveCount = 1;

        for (int i = col + 1; i < ColumnCount && consecutiveCount < length; i++)
        {
            if (_grid[row, i] == currentElement)
            {
                consecutiveCount++;
            }
            else
            {
                break;
            }
        }

        for (int i = col - 1; i >= 0 && consecutiveCount < length; i--)
        {
            if (_grid[row, i] == currentElement)
            {
                consecutiveCount++;
            }
            else
            {
                break;
            }
        }

        return consecutiveCount >= length;
    }
    public bool IsColumn(int row, int col, int length)
    {
        if (row < 0 || row > RowCount || col < 0 || col > ColumnCount)
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        int currentElement = _grid[row, col];
        int consecutiveCount = 1;
        
        for (int i = row - 1; i >= 0 && consecutiveCount < length; i--)
        {
            if (_grid[i, col] == currentElement)
            {
                consecutiveCount++;
            }
            else
            {
                break;
            }
        }
        
        for (int i = row + 1; i < RowCount && consecutiveCount < length; i++)
        {
            if (_grid[i, col] == currentElement)
            {
                consecutiveCount++;
            }
            else
            {
                break;
            }
        }

        return consecutiveCount >= length;
    }
    public bool IsReverseDiagonal(int row, int col, int length)
    {
        if (row < 0 || row > RowCount || col < 0 || col > ColumnCount)
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        int currentElement = _grid[row, col];
        int consecutiveCount = 1;
        
        for (int i = row - 1, j = col + 1; i >= 0 && j < ColumnCount && consecutiveCount < length; i--, j++)
        {
            if (_grid[i, j] == currentElement)
            {
                consecutiveCount++;
            }
            else
            {
                break;
            }
        }
        
        for (int i = row + 1, j = col - 1; i < RowCount && j >= 0 && consecutiveCount < length; i++, j--)
        {
            if (_grid[i, j] == currentElement)
            {
                consecutiveCount++;
            }
            else
            {
                break;
            }
        }

        return consecutiveCount >= length;
    }

    public bool IsDiagonal(int row, int col, int length)
    {
        if (row < 0 || row > RowCount || col < 0 || col > ColumnCount)
        {
            throw new ArgumentOutOfRangeException("Invalid row or column index.");
        }

        int currentElement = _grid[row, col];
        int consecutiveCount = 1;
        
        for (int i = row - 1, j = col - 1; i >= 0 && j >= 0 && consecutiveCount < length; i--, j--)
        {
            if (_grid[i, j] == currentElement)
            {
                consecutiveCount++;
            }
            else
            {
                break;
            }
        }
        
        for (int i = row + 1, j = col + 1; i < RowCount && j < ColumnCount && consecutiveCount < length; i++, j++)
        {
            if (_grid[i, j] == currentElement)
            {
                consecutiveCount++;
            }
            else
            {
                break;
            }
        }


        return consecutiveCount >= length;
    }


    public void PrintList(List<int> list)
    {
        string list_S = string.Empty;
        foreach (var t in list)
        {
            list_S += t.ToString() + "  ";
        }
        Debug.Log(list_S);
    }
    public void PrintMatrix()
    {
        string matrix = string.Empty;
        for (int i = 0; i < RowCount; i++)
        {
            matrix += "\n";
            for (int j = 0; j < ColumnCount; j++)
            {
                matrix += $"{_grid[i,j]} " ;
            }
        }
        Debug.Log(matrix);
    }


    public virtual void SetElementAt(int row, int col, int status) => _grid[row,col] = status;

    private bool ValidateRowPosition(int row) => row >= 0 && row < RowCount;
    private bool ValidateColPosition(int col) => col >= 0 && col < ColumnCount;


    public int GetElement(int row, int col)
    {
        int val = -1;
        if (ValidateColPosition(col) && ValidateRowPosition(row))
            val = _grid[row,col];

        return val;
    }

    

}
