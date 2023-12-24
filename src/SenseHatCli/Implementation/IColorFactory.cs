using System.Drawing;

namespace SenseHatCli.Implementaiton;

internal interface IColorFactory
{
    ReadOnlySpan<Color> GetRandomColors(int count = 64);
}