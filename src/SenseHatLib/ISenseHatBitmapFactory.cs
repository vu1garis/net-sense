using System;
using System.Collections;

namespace SenseHatLib;

public interface ISenseHatBitmapFactory
{
    bool ContainsKey(char character);

    BitArray? GetBitMap(char character);
}
