using Microsoft.Extensions.DependencyInjection;

using SenseHatLib;
using SenseHatLib.Implementation;

namespace SenseHatLibTest;

public class UnixFortuneTests
{
    [Fact]
    public void GetRandomFortuneSuccess()
    {
        var sc = new ServiceCollection();

        sc.AddSenseHatServices();

        using var sp = sc.BuildServiceProvider();

        var actual = sp.GetRequiredService<IUnixFortune>();

        Assert.NotNull(actual);

        var fortune = actual.Next();

        Assert.NotNull(fortune);
    }
}
