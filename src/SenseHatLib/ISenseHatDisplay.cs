using System.Drawing;

namespace SenseHatLib;

public interface ISenseHatDisplay
{
    Task Clear();

    Task Fill(Color color); 
    
    Task Fill(IEnumerable<Color> colors);

    Task DisplayFortune(
        Color foreground, 
        Color background, 
        bool loop = false, 
        int delay = 1000);

    Task DisplayText(
        string text,
        Color foreground,
        Color background,
        bool loop = false,
        bool scroll = false,
        int delay = 1000);
}