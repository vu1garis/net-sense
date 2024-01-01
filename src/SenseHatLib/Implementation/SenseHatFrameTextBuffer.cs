using System;
using System.Collections.Generic;
using System.Drawing;

namespace SenseHatLib.Implementation;

internal sealed class SenseHatFrameTextBuffer : ISenseHatFrameTextBuffer
{
    private readonly Queue<SenseHatFrame> _buffer = new Queue<SenseHatFrame>();

    private readonly ISenseHatBitmapFactory _bitmapFactory;

    public SenseHatFrameTextBuffer(ISenseHatBitmapFactory factory)
    {
        _bitmapFactory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public bool IsLooping { get; set; }

    public void Append(string text, Color foreground, Color background)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(nameof(text));
        }

        // convert the text to a series of frames in a FIFO queue
        foreach (var c in text)
        {
            var bm = _bitmapFactory.GetBitMap(c) ?? throw new InvalidOperationException($"Character {c} not currently supported");

            var frame = bm.Color(foreground: foreground, background: background);

            var shf = new SenseHatFrame();

            shf.Set(frame);

            _buffer.Enqueue(shf);
        }
    }

    public ISenseHatFrame? Next()
    {
        SenseHatFrame? next = _buffer.Dequeue();

        if (next != null && IsLooping)
        {
            _buffer.Enqueue(next);
        }

        return next;
    } 
}