using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;

namespace SenseHatLib.Implementation;

internal sealed class SenseHatFrameTextBuffer : ISenseHatFrameTextBuffer
{
    private readonly Queue<ISenseHatFrame> _buffer = new Queue<ISenseHatFrame>();

    private readonly ISenseHatFrameFactory _factory;

    public SenseHatFrameTextBuffer(ISenseHatFrameFactory factory)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public bool IsLooping { get; set; }

    public bool IsScrolling { get; set; }

    public void Clear()
    {
        _buffer.Clear();
    }

    public void Append(string text, Color foreground, Color background)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(text));
        }

        var frames = _factory.Create(text, foreground, background);

        if (IsScrolling)
        {
            AppendScrollingFrames(frames);
        }
        else
        {
            AppendFrames(frames);
        }
    }

    public ISenseHatFrame? Next()
    {
        ISenseHatFrame? next = _buffer.Count > 0 ? _buffer.Dequeue() : default;

        if (next != null && IsLooping)
        {
            _buffer.Enqueue(next);
        }

        return next;
    } 

    private void AppendScrollingFrames(IList<ISenseHatFrame> frames)
    {
        for (int i = 0; i < frames.Count; i++)
        {
            var current = frames[i];

            var next = i == frames.Count - 1 ? default : frames[i + 1];

            if (next != default)
            {
                for (int j = 1; j < SenseHatFrame.SENSEHAT_MAX_COLUMNS; j++)
                {
                    var df = current
                        .Select(rowFilter:.., columnFilter:j..)
                        .AppendColumns(next.Select(rowFilter:.., columnFilter:..j));

                    _buffer.Enqueue(df);
                }
            }
            else
            {
                _buffer.Enqueue(current);
            }
        }
    }

    private void AppendFrames(IList<ISenseHatFrame> frames)
    {
        for (int i = 0; i < frames.Count; i++)
        {
            _buffer.Enqueue(frames[i]);
        }
    }
}