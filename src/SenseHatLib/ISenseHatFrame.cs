using System.Drawing;

namespace SenseHatLib;

public interface ISenseHatFrame
{
    int RowCount { get; }

    int ColumnCount { get; }

    int Size { get; }

    Color this[int row, int column] { get; set; }

    Color Get(int row, int column);

    void Set(int row, int column, Color color);

    Color[] ToArray();

    void Set(IList<Color> colors);
    
    ISenseHatFrame Select(Range rowFilter, Range columnFilter);
}