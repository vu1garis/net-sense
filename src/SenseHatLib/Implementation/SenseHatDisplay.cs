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

    public async Task Clear() 
        => await _client.Clear().ConfigureAwait(false);

    public async Task Fill(Color color) 
        => await _client.Fill(color).ConfigureAwait(false);
    
    public async Task Fill(IEnumerable<Color> colors) 
        => await _client.Fill(colors.ToArray()).ConfigureAwait(false);

    public async Task DisplayText(string text, Color foreground, Color background, bool loop = false, bool scroll = false, int delay = 1000)
    {
        ValidateParameters(text, delay);

        _textBuffer.Clear();
        _textBuffer.IsLooping = loop;
        _textBuffer.IsScrolling = scroll;
        _textBuffer.Append(text, foreground, background);

        await _client.Clear().ConfigureAwait(false);

        ISenseHatFrame? current = null;

        while (true)
        {
            current = _textBuffer.Next();

            if (current == null)
            {
                break;
            }

            await _client.Fill(current.ToArray()).ConfigureAwait(false);

            await Task.Delay(delay).ConfigureAwait(false);
        }
    }

    private void ValidateParameters(string text, int delay)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Error invalid display text", nameof(text));
        }

        if (delay < 0)
        {
            throw new ArgumentException("Error, invalid delay expecting a millisecond value > 0", nameof(delay));
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