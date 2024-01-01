using System.Collections.Generic;
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

        if (scroll)
        {
            ScrollCharacters(text, foreground, background, loop, delay);
        }
        else
        {
            WriteCharacters(text, foreground, background, loop, delay);
        }
    }

    private void ScrollCharacters(string text, Color foreground, Color background, bool loop, int delay)
    {
        var frames = new Queue<SenseHatFrame>();

        // convert the test to a series of frames in a FIFO queue
        foreach (var c in text)
        {
            var bm = _bitmapFactory.GetBitMap(c) ?? throw new InvalidOperationException($"Character {c} not currently supported");

            var frame = bm.Color(foreground: foreground, background: background);

            var shf = new SenseHatFrame();

            shf.Set(frame);

            frames.Enqueue(shf);
        }

        SenseHatFrame? current = null;


        while (true)
        {
            var next = frames.Dequeue();

            if (next == null && loop)
            {
                throw new InvalidOperationException("Error, frame queue empty and loop == true");
            }

            if (current == null)
            {
                // first character to display...
                current = next;

                _client.Fill(current.ToReadOnlySpan());

                Thread.Sleep(delay);
            }
            else
            {
                // we need to slowly replace current with next column by column
                // until next becomes current...
                for (int i = 1; i < SenseHatFrame.SENSEHAT_MAX_COLUMNS; i++)
                {
                    var df = current
                        .Select(rowFilter:.., columnFilter:i..)
                        .AppendColumns(next.Select(rowFilter:.., columnFilter:..i));

                    _client.Fill(df.ToReadOnlySpan());

                    Thread.Sleep(delay);
                }

                current = next;
            }

            if (loop)
            {
                frames.Enqueue(current);
            }
            else
            {
                break;
            }
        }
    }

    private void WriteCharacters(string text, Color foreground, Color background, bool loop, int delay)
    {
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

                    if (loop && !cache.ContainsKey(c))
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