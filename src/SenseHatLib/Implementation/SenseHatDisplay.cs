using System.Drawing;

namespace SenseHatLib.Implementation;

internal sealed class SenseHatDisplay : ISenseHatDisplay, IDisposable
{
    private bool _disposed;

    private readonly ISenseHatClient _client; 
    private readonly ISenseHatBitmapFactory _bitmapFactory;

    public SenseHatDisplay(ISenseHatClient client, ISenseHatBitmapFactory bitmapFactory) 
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _bitmapFactory = bitmapFactory ?? throw new ArgumentNullException(nameof(bitmapFactory));
    }

    public void Clear() => _client.Clear();

    public void Fill(Color color) => _client.Fill(color);
    
    public void Fill(ReadOnlySpan<Color> colors) => _client.Fill(colors);

    public void DisplayText(
        string text,
        Color foreground,
        Color background,
        bool loop = false,
        bool scroll = false,
        int delay = 1000)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(text));
        }

        if (delay < 0)
        {
            throw new ArgumentException(nameof(delay));
        }

        while (true)
        {
            var cache = new Dictionary<char, Color[]>();

            foreach (var c in text)
            {
                if (cache.ContainsKey(c))
                {
                    _client.Fill(cache[c]);
                }
                else
                {
                    var bm = _bitmapFactory.GetBitMap(c) ?? throw new InvalidOperationException($"Character {c} not currently supported");

                    var frame = bm.Color(foreground: foreground, background: background);

                    _client.Fill(frame);

                    if (loop)
                    {
                        cache[c] = frame;
                    }
                }

                Thread.Sleep(delay);
            }

            if (!loop)
            {
                break;
            }
        }
    }

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            try
            {
                if (disposing)
                {
                    _client.Dispose();
                }
            }
            finally
            {
                _disposed = true;
            }
        }
    }

    #endregion
}