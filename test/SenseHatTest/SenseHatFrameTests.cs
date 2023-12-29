using System.Collections;
using System.Drawing;
using SenseHatCli.Implementaiton;

namespace SenseHatTest;

public class SenseHatFrameTests
{
    [Fact]
    public void SenseHatFrameFromBitArraySuccess()
    {
        var br = new BitArray(new bool[]
        { 
            false, false, false, false,
            false, true,  true,  false,
            false, true,  true,  false,
            false, false, false, false
        });

        var colors = br.Color(foreground: Color.Red, background: Color.Blue);

        var actual = new SenseHatFrame(rows: 4, columns: 4);

        actual.Set(colors);

        Assert.Equal(4, actual.RowCount);
        Assert.Equal(4, actual.ColumnCount);
        Assert.Equal(16, actual.Size);

        // row 0
        Assert.Equal(Color.Blue, actual[0,0]);
        Assert.Equal(Color.Blue, actual[0,1]);
        Assert.Equal(Color.Blue, actual[0,2]);
        Assert.Equal(Color.Blue, actual[0,3]);

        // row 1
        Assert.Equal(Color.Blue, actual[1,0]);
        Assert.Equal(Color.Red,  actual[1,1]);
        Assert.Equal(Color.Red,  actual[1,2]);
        Assert.Equal(Color.Blue, actual[1,3]);

        // row 2
        Assert.Equal(Color.Blue, actual[2,0]);
        Assert.Equal(Color.Red,  actual[2,1]);
        Assert.Equal(Color.Red,  actual[2,2]);
        Assert.Equal(Color.Blue, actual[2,3]);

        // row 3
        Assert.Equal(Color.Blue, actual[3,0]);
        Assert.Equal(Color.Blue, actual[3,1]);
        Assert.Equal(Color.Blue, actual[3,2]);
        Assert.Equal(Color.Blue, actual[3,3]);                        
    }

    [Fact]
    public void SenseHatFrameSelectSuccess()
    {
        var br = new BitArray(new bool[]
        { 
            false, false, false, false,
            false, true,  true,  false,
            false, true,  true,  false,
            false, false, false, false
        });

        var colors = br.Color(foreground: Color.Red, background: Color.Blue);

        var frame = new SenseHatFrame(rows: 4, columns: 4);

        frame.Set(colors);

        var actual = frame.Select(rowFilter: 1..3, columnFilter: 1..3);

        Assert.Equal(2, actual.RowCount);
        Assert.Equal(2, actual.ColumnCount);
        Assert.Equal(4, actual.Size);

        // row 0
        Assert.Equal(Color.Red, actual[0,0]);
        Assert.Equal(Color.Red, actual[0,1]);
 
        // row 1
        Assert.Equal(Color.Red,  actual[1,0]);
        Assert.Equal(Color.Red,  actual[1,1]);                        
    }
}