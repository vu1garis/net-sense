using System.Collections.Generic;
using System.Drawing;

namespace SenseHatLib.Implementation;

internal sealed class SenseHatDisplay : ISenseHatDisplay, IDisposable
{
    private bool _disposed;

    private readonly ISenseHatClient _client; 
    private readonly ISenseHatFrameTextBuffer _textBuffer;

    public SenseHatDisplay(ISenseHatClient client, ISenseHatFrameTextBuffer textBuffer) 
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));

        _textBuffer = textBuffer ?? throw new ArgumentNullException(nameof(textBuffer));
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

        _textBuffer.Append(text, foreground, background);

        _textBuffer.IsLooping = loop;

        ISenseHatFrame? current = null;

        while (true)
        {
            var next = _textBuffer.Next();

            if (next == null && loop)
            {
                throw new InvalidOperationException("Error, frame buffer empty and loop == true");
            }

            if (scroll)
            {
                if (current == null)
                {
                    // first character to display...
                    current = next;

                    _client.Fill(current.ToReadOnlySpan());

                    Thread.Sleep(delay);
                }
                else
                {
                    // we need to slowly replace current with next, column by column
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
            }
            else
            {
                // just display the character
                _client.Fill(next.ToReadOnlySpan());

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