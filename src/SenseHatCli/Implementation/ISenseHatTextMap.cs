using System;
using System.Collections;

namespace SenseHatCli.Implementaiton;

internal interface ISenseHatTextMap
{
    bool ContainsKey(char character);

    BitArray? GetBitMap(char character);
}
