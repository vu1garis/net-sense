using System.Drawing;

namespace SenseHatLib;

public interface IColorFactory
{
    ReadOnlySpan<Color> GetRandomColors(int count = 64);
}