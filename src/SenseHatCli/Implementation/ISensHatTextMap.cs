using System;
using System.Collections;

namespace SenseHatCli.Implementaiton;

internal interface ISensHatTextMap
{
    bool ContainsKey(char character);

    BitArray? GetBitMap(char character);
}
