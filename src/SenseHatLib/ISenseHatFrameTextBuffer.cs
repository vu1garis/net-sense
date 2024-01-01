using System.Drawing;

namespace SenseHatLib;

public interface ISenseHatFrameTextBuffer
{
    bool IsLooping { get; set; }

    void Append(string text, Color foreground, Color background);

    ISenseHatFrame? Next();
}