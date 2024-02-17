using System.Drawing;

namespace SenseHatLib.Implementation;

public class SenseHatFrameFactory : ISenseHatFrameFactory
{
    private readonly ISenseHatBitmapFactory _bitmapFactory;

    public SenseHatFrameFactory(ISenseHatBitmapFactory factory)
    {
        _bitmapFactory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public IList<ISenseHatFrame> Create(
        string text, 
        Color foreground, 
        Color background)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentNullException(nameof(text));
        }

        var frames = new List<ISenseHatFrame>();

        foreach (var c in text)
        {
            var bm = _bitmapFactory.GetBitMap(c) ?? throw new InvalidOperationException($"Character {c} not currently supported");

            var frame = bm.Color(foreground: foreground, background: background);

            var shf = new SenseHatFrame();

            shf.Set(frame);

            frames.Add(shf);
        }

        return frames;
    }
}
