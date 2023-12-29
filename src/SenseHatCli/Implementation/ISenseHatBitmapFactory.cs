using System;
using System.Collections;

namespace SenseHatCli.Implementaiton;

internal interface ISenseHatBitmapFactory
{
    bool ContainsKey(char character);

    BitArray? GetBitMap(char character);
}
