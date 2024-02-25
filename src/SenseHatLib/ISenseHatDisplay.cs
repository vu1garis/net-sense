using System.Drawing;

namespace SenseHatLib;

public interface ISenseHatDisplay
{
    void Clear();

    void Fill(Color color); 
    
    void Fill(IEnumerable<Color> colors);

    void DisplayFortune(
        Color foreground, 
        Color background, 
        bool loop = false, 
        int delay = 1000);

    void DisplayText(
        string text,
        Color foreground,
        Color background,
        bool loop = false,
        bool scroll = false,
        int delay = 1000);
}