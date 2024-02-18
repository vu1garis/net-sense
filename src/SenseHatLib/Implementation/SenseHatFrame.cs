using System;
using System.Drawing;

namespace SenseHatLib.Implementation;

internal sealed class SenseHatFrame : ISenseHatFrame
{
    public const int SENSEHAT_MAX_ROWS = 8;

    public const int SENSEHAT_MAX_COLUMNS = 8;

    private readonly static Color COLOR_OFF = Color.Empty;

    private readonly Color[][] _frame;

    private readonly int _rowCount;

    private readonly int _columnCount;

    private readonly int _size;

    public SenseHatFrame()
        : this(rows: SENSEHAT_MAX_ROWS, columns: SENSEHAT_MAX_COLUMNS)
    {
    }

    public SenseHatFrame(int rows, int columns)
    {
        if (rows <= 0)
        {
            throw new ArgumentException(nameof(rows));
        }

        if (columns <= 0)
        {
            throw new ArgumentException(nameof(columns));
        }

        _rowCount = rows;

        _columnCount = columns;

        _size = _rowCount * _columnCount;

        _frame = new Color[_rowCount][];

        for (int r = 0; r < _rowCount; r++)
        {
            _frame[r] = new Color[_columnCount];

            for (int c = 0; c < _columnCount; c++)
            {
                _frame[r][c] = COLOR_OFF;
            }
        }
    }

    public int RowCount => _rowCount;

    public int ColumnCount => _columnCount;

    public int Size => _size;

    public Color this[int row, int column]
    {
        get => Get(row, column);
        set => Set(row, column, value);
    }

    public Color Get(int row, int column)
    {
        this.AssertInRange(row, column);

        return _frame[row][column];
    }

    public void Set(int row, int column, Color color)
    {
        this.AssertInRange(row, column);

        _frame[row][column] = color;
    }

    public Color[] ToArray()
    {
        var res = new Color[RowCount * ColumnCount];

        int index = 0;

        for (int r = 0; r < RowCount; r++)
        {
            for (int c = 0; c < ColumnCount; c++)
            {
                res[index] = _frame[r][c];

                index++;
            }
        }

        return res;
    }

    public void Set(IList<Color> colors)
    {
        if (colors == null || colors.Count > Size)
        {
            throw new InvalidOperationException($"Color[] must be non-null and no bigger than {Size}");
        }

        int index = 0;

        for (int r = 0; r < RowCount; r++)
        {
            for (int c = 0; c < ColumnCount; c++)
            {
                _frame[r][c] = colors[index];

                index++;
            }
        }
    }

    public ISenseHatFrame Select(Range rowFilter, Range columnFilter)
    {
        this.AssertInRange(rowFilter, columnFilter);

        var rowCount = rowFilter.GetOffsetAndLength(RowCount).Item2;
        var columnCount = columnFilter.GetOffsetAndLength(ColumnCount).Item2;

        var subFrame = new SenseHatFrame(rows: rowCount, columns: columnCount);

        var subR = 0;
        var subC = 0;

        var selectedRows = _frame[rowFilter];

        foreach (var row in selectedRows)
        {
            var selectedColumns = row[columnFilter];

            foreach (var cell in selectedColumns)
            {
                subFrame.Set(subR, subC, cell);

                subC++;
            }

            subR++;
            subC = 0;
        }

        return subFrame;
    }
}