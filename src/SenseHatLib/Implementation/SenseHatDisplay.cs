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

    public void DisplayText(string text, Color foreground, Color background, bool loop = false, bool scroll = false, int delay = 1000)
    {
        ValidateParameters(text, delay);

        _textBuffer.Append(text, foreground, background);
        _textBuffer.IsLooping = loop;

        _client.Clear();

        ISenseHatFrame? current = null;

        while (true)
        {
            var next = _textBuffer.Next();

            if (ShouldExitLoop(next, loop))
            {
                break;
            }

            if (scroll)
            {
#pragma warning disable CS8604 // Possible null reference argument, ShouldExitLoop asserts null 
                current = DisplayScrollFrames(current, next);
#pragma warning restore CS8604 // Possible null reference argument.
            }
            else
            {
#pragma warning disable CS8604 // Possible null reference argument, ShouldExitLoop asserts null
                DisplayFrame(next);
#pragma warning restore CS8604 // Possible null reference argument.
            }

            Thread.Sleep(delay);
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

    private bool ShouldExitLoop(ISenseHatFrame? next, bool loop)
    {
        if (next == null)
        {
            if (loop)
            {
                throw new InvalidOperationException("Error, frame buffer empty and loop == true");
            }
            else
            {
                // there are no more frames in the buffer and we are not 
                // looping so we just need to exit
                return true;
            }
        }

        return false;
    }

    private ISenseHatFrame? DisplayScrollFrames(ISenseHatFrame? current, ISenseHatFrame next)
    {
        ISenseHatFrame? latestFrame = null;

        if (current == null)
        {
            // current is used to keep track of the current frame.
            // if we are in scrolling mode and the current frame is null
            // it means we are about to display the first frame in which
            // case we don't need to worry about scrolling and just
            // set current to next, i.e. next is the first and current frame
            latestFrame = next;

            DisplayFrame(latestFrame);
        }
        else
        {
            // current is not null this means that it currently occupies the
            // full display. We now we need to slowly replace current with next's
            // pixels column by column until next becomes current.
            latestFrame = ReplaceCurrentWithNext(current, next);
        }

        return latestFrame;
    }

    private ISenseHatFrame ReplaceCurrentWithNext(ISenseHatFrame current, ISenseHatFrame next)
    {
        for (int i = 1; i < SenseHatFrame.SENSEHAT_MAX_COLUMNS; i++)
        {
            var df = current
                .Select(rowFilter:.., columnFilter:i..)
                .AppendColumns(next.Select(rowFilter:.., columnFilter:..i));

            DisplayFrame(df);
        }

        // once the for loop above has exited then the frame occupying
        // the display will be equivalent to the frame next extracted from
        // the buffer. return this frame (next) as the latest frame
        return next;
    }

    private void DisplayFrame(ISenseHatFrame frame)
    {
        // just display the character
        _client.Fill(frame.ToReadOnlySpan());    
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