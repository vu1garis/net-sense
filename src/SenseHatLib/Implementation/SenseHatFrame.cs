using System;
using System.Drawing;


namespace SenseHatLib.Implementation;

internal sealed class SenseHatFrame
{
    private const int SENSEHAT_MAX_ROWS = 8;

    private const int SENSEHAT_MAX_COLUMNS = 8;

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
        AssertInRange(row, column);

        return _frame[row][column];
    }

    public void Set(int row, int column, Color color)
    {
        AssertInRange(row, column);

        _frame[row][column] = color;
    }

    public void Set(Color[] colors)
    {
        if (colors == null || colors.Length > Size)
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

    public SenseHatFrame Select(Range rowFilter, Range columnFilter)
    {
        AssertInRange(rowFilter, columnFilter);

        var subFrame = new SenseHatFrame(
            rows: rowFilter.End.Value - rowFilter.Start.Value, 
            columns: columnFilter.End.Value - columnFilter.Start.Value);

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

    private void AssertInRange(Range rows, Range columns)
    {
        if (rows.Start.Value < 0 || rows.End.Value >= RowCount)
        {
            throw new IndexOutOfRangeException($"Rows range {rows} must be in the range 0 - {RowCount - 1}!");
        }

        if (columns.Start.Value < 0 || columns.End.Value >= ColumnCount)
        {
            throw new IndexOutOfRangeException($"Columns range {columns} must be in the range 0 - {ColumnCount - 1}!");
        }
    }

    private void AssertInRange(int row, int column)
    {
        if (row < 0 || row >= RowCount)
        {
            throw new IndexOutOfRangeException($"Row {row} must be in the range 0 - {RowCount - 1}!");
        }

        if (column < 0 || column >= ColumnCount)
        {
            throw new IndexOutOfRangeException($"Column {column} must be in the range 0 - {ColumnCount - 1}!");
        }
    }
}