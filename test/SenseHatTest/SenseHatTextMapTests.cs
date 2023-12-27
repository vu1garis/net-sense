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

        var actual = sp.GetRequiredService<ISensHatTextMap>();

        Assert.NotNull(actual);
    }
}