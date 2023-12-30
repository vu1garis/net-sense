using System.Collections;
using System.Drawing;

using SenseHatLib.Implementation;

namespace SenseHatLibTest;

public class BitArrayExtensionTests
{
    [Fact]
    public void BitMapColorSuccess()
    {
        var bm = new BitArray(new bool[4] { true, false, true, false });

        var span = bm.Color(Color.White, Color.Black);

        Assert.Equal(Color.White, span[0]);
        Assert.Equal(Color.Black, span[1]);
        Assert.Equal(Color.White, span[2]);
        Assert.Equal(Color.Black, span[3]);
    }
}