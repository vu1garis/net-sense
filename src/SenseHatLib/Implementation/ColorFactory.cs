using System.Drawing;

namespace SenseHatLib.Implementation;

internal sealed class ColorFactory : IColorFactory
{
    private Lazy<int[]> _valid = new Lazy<int[]>(() => Enumerable.Range(0, 256).ToArray());

    public ReadOnlySpan<Color> GetRandomColors(int count = 64)
    {
        if (count <= 0)
        {
            throw new ArgumentException(nameof(count));
        }

        var rnd = Random.Shared.GetItems(_valid.Value, count * 3);
        
        var span = new Color[count];

        for (int i = 0; i < count; i++)
        {
            int offset = i * 3;

            span[i] = Color.FromArgb(
                red: rnd[offset], 
                green: rnd[offset+1], 
                blue: rnd[offset+2]);
        }

        return span;
    }
}