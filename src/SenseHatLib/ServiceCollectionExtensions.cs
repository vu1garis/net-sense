using Microsoft.Extensions.DependencyInjection;

using SenseHatLib.Implementation;

namespace SenseHatLib;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSenseHatServices(this IServiceCollection sc)
    {
        sc.AddSingleton<ISenseHatClient, SenseHatClient>();
        sc.AddSingleton<IColorFactory, ColorFactory>();
        sc.AddSingleton<ISenseHatBitmapFactory, SenseHatBitmapFactory>();

        return sc;
    }
}