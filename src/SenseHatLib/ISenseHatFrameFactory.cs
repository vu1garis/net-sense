using System.Drawing;

namespace SenseHatLib;

public interface ISenseHatFrameFactory
{
    IList<ISenseHatFrame> Create(
        string text, 
        Color foreground, 
        Color background);
}
