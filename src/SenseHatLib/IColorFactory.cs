using System.Drawing;

namespace SenseHatLib;

public interface IColorFactory
{
    Color[] GetRandomColors(int count = 64);
}