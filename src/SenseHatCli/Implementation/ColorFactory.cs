using System.Drawing;

namespace SenseHatCli.Implementaiton;

internal sealed class ColorFactory : IColorFactory
{
    private const int MAX_64_COLOR = 192;

    private Lazy<int[]> _valid = new Lazy<int[]>(() => Enumerable.Range(0, 256).ToArray());

    public ReadOnlySpan<Color> GetRandomColors(int count = 64)
    {
        var rnd = Random.Shared.GetItems(_valid.Value, MAX_64_COLOR);
        
        var span = new Color[count];

        for (int i = 0; i < count; i++)
        {
            int offset = i * 3;

            span[i] = Color.FromArgb(rnd[offset], rnd[offset+1], rnd[offset+2]);
        }

        return span;
    }
}