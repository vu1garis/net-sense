using System.Collections;
using System.Drawing;
using System.Linq;

namespace SenseHatCli.Implementaiton;

internal static class BitArrayExtensions
{
    public static ReadOnlySpan<Color> Color(this BitArray array, Color foreground, Color background)
    {
        if (array == null)
        {
            throw new ArgumentException(nameof(array));
        }

        return array.Cast<bool>().Select(i => i ? foreground : background).ToArray();
    }
}