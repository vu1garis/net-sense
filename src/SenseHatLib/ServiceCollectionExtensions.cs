using Microsoft.Extensions.DependencyInjection;

using SenseHatLib.Implementation;

namespace SenseHatLib;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSenseHatServices(this IServiceCollection sc)
    {
        sc.AddSingleton<ISenseHatBitmapFactory, SenseHatBitmapFactory>();
        sc.AddSingleton<IColorFactory, ColorFactory>();
        sc.AddSingleton<ISenseHatClient, SenseHatClient>();
        sc.AddSingleton<ISenseHatDisplay, SenseHatDisplay>();

        sc.AddTransient<ISenseHatFrameTextBuffer, SenseHatFrameTextBuffer>();

        return sc;
    }
}