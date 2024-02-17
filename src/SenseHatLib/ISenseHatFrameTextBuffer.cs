using System.Drawing;

namespace SenseHatLib;

public interface ISenseHatFrameTextBuffer
{
    bool IsLooping { get; set; }

    bool IsScrolling { get; set; }

    void Clear();

    void Append(string text, Color foreground, Color background);

    ISenseHatFrame? Next();
}