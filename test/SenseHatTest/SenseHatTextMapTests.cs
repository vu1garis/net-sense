using Microsoft.Extensions.DependencyInjection;
using SenseHatCli.Implementaiton;

namespace SenseHatTest;

public class SenseHatTextMapTests
{
    [Fact]
    public void TextMapCompositionSuccess()
    {
        var sc = new ServiceCollection();

        sc.AddCommandServices();

        using var sp = sc.BuildServiceProvider();

        var actual = sp.GetRequiredService<ISenseHatTextMap>();

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
    public void TextMapGetBitMapTheorySuccess(char value)
    {
        var sc = new ServiceCollection();

        sc.AddCommandServices();

        using var sp = sc.BuildServiceProvider();

        var map = sp.GetRequiredService<ISenseHatTextMap>();

        var actual = map.GetBitMap(value);

        Assert.NotNull(actual);

        Assert.Equal(64, actual.Length);
    }
}