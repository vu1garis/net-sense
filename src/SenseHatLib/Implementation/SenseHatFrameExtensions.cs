using System;
using System.Drawing;

namespace SenseHatLib.Implementation;

internal static class SenseHatFrameExtensions
{
    public static void AssertInRange(this ISenseHatFrame frame, Range rows, Range columns)
    {
        if (rows.Start.Value < 0 || rows.End.Value >= frame.RowCount)
        {
            throw new IndexOutOfRangeException($"Rows range {rows} must be in the range 0 - {frame.RowCount - 1}!");
        }

        if (columns.Start.Value < 0 || columns.End.Value >= frame.ColumnCount)
        {
            throw new IndexOutOfRangeException($"Columns range {columns} must be in the range 0 - {frame.ColumnCount - 1}!");
        }
    }

    public static void AssertInRange(this ISenseHatFrame frame, int row, int column)
    {
        if (row < 0 || row >= frame.RowCount)
        {
            throw new IndexOutOfRangeException($"Row {row} must be in the range 0 - {frame.RowCount - 1}!");
        }

        if (column < 0 || column >= frame.ColumnCount)
        {
            throw new IndexOutOfRangeException($"Column {column} must be in the range 0 - {frame.ColumnCount - 1}!");
        }
    }

    public static SenseHatFrame AppendColumns(this ISenseHatFrame me, ISenseHatFrame appendFrom)
    {
        if (me.RowCount != appendFrom.RowCount)
        {
            throw new InvalidOperationException("Error, AppendColumns can only be used on frames with equal numbers of rows!");
        }

        var res = new SenseHatFrame(rows: me.RowCount, columns: me.ColumnCount + appendFrom.ColumnCount);

        for (int r = 0; r < me.RowCount; r++)
        {
            for (int lc = 0; lc < me.ColumnCount; lc++)
            {
                res[r, lc] = me[r, lc];
            }

            for (int rc = 0; rc < appendFrom.ColumnCount; rc++)
            {
                res[r, rc + me.ColumnCount] = appendFrom[r, rc];
            }
        }

        return res;
    }
    
    public static SenseHatFrame AppendRows(this ISenseHatFrame me, ISenseHatFrame appendFrom)
    {
        if (me.ColumnCount != appendFrom.ColumnCount)
        {
            throw new InvalidOperationException("Error, AppendRows can only be used on frames with equal numbers of columns!");
        }

        var res = new SenseHatFrame(rows: me.RowCount + appendFrom.ColumnCount, columns: me.ColumnCount);

        for (int c = 0; c < me.ColumnCount; c++)
        {
            for (int lr = 0; lr < me.RowCount; lr++)
            {
                res[lr, c] = me[lr, c];
            }

            for (int rr = 0; rr < appendFrom.RowCount; rr++)
            {
                res[rr + me.RowCount, c] = me[rr, c];
            }
        }

        return res;
    } 
}