using System.Drawing;

namespace SenseHatLib;

public interface ISenseHatDisplay
{
    void Clear();

    void Fill(Color color); 
    
    void Fill(ReadOnlySpan<Color> colors);

    void DisplayText(
        string text,
        Color foreground,
        Color background,
        bool loop = false,
        bool scroll = false,
        int delay = 1000);
}