
using System.Collections.Generic;

public class BlockMatrix : Matrix
{
    private Block[,] _Blocks;
    public BlockMatrix(int row, int col, int val) : base(row, col, val)
    {
        _Blocks = new Block[row, col];
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                _Blocks[i, j] = null;
                
            }
        }
    }

    public List<Block> GetRow(int row)
    {
        List<Block> blocks = new();
        for (int i = 0; i < ColumnCount; i++)
        {
            blocks.Add(_Blocks[row,i]);
        }

        return blocks;
    }

    public List<Block> GetCol(int col)
    {
        List<Block> blocks = new();
        for (int i = 0; i < RowCount; i++)
        {
            blocks.Add(_Blocks[i,col]);
        }

        return blocks;
    }

    public void InitializeBlocks(int x, int y, Block b)
    {
        _Blocks[x, y] = b;
    }
    public Block GetBlock(int x, int y) => _Blocks[x,y];

    public override void SetElementAt(int row, int col, int status)
    {
        base.SetElementAt(row, col, status);
        _Blocks[row, col].SetStatus(status);
    }

    public void Reset()
    {
        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColumnCount; j++)
            {
                _Blocks[i, j].SetStatus(0);
            }
        }
    }
}
