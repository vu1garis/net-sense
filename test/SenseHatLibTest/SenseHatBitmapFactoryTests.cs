using Microsoft.Extensions.DependencyInjection;

using SenseHatLib;
using SenseHatLib.Implementation;

namespace SenseHatLibTest;

public class SenseHatBitmapFactoryTests
{
    [Fact]
    public void FrameMapCompositionSuccess()
    {
        var sc = new ServiceCollection();

        sc.AddSenseHatServices();

        using var sp = sc.BuildServiceProvider();

        var actual = sp.GetRequiredService<ISenseHatBitmapFactory>();

        Assert.NotNull(actual);
    }

    [Theory]
    [InlineData('A')]
    [InlineData('B')]
    [InlineData('C')]
    [InlineData('D')]
    [InlineData('E')]
    [InlineData('F')]    
    [InlineData('G')]
    [InlineData('H')]
    [InlineData('I')]
    [InlineData('J')]
    [InlineData('K')]    
    [InlineData('L')]
    [InlineData('M')]
    [InlineData('N')]
    [InlineData('O')]
    [InlineData('P')]
    [InlineData('Q')]
    [InlineData('R')]
    [InlineData('S')]
    [InlineData('T')]
    [InlineData('U')]
    [InlineData('V')]
    [InlineData('W')]
    [InlineData('X')]
    [InlineData('Y')]
    [InlineData('Z')]
    [InlineData(' ')]    
    [InlineData('0')]
    [InlineData('1')]
    [InlineData('2')]
    [InlineData('3')]
    [InlineData('4')]
    [InlineData('5')]    
    [InlineData('6')]
    [InlineData('7')]
    [InlineData('8')]
    [InlineData('9')] 
    [InlineData('.')]
    [InlineData('°')]
    [InlineData('%')]  
    [InlineData('-')]
    [InlineData('_')]
    [InlineData('=')]  
    [InlineData('!')]
    public void FrameMapGetBitMapTheorySuccess(char value)
    {
        var sc = new ServiceCollection();

        sc.AddSenseHatServices();

        using var sp = sc.BuildServiceProvider();

        var map = sp.GetRequiredService<ISenseHatBitmapFactory>();

        var actual = map.GetBitMap(value);

        Assert.NotNull(actual);

        Assert.Equal(64, actual.Length);
    }
}