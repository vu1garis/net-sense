using Microsoft.Extensions.DependencyInjection;

namespace SenseHatCli.Implementaiton;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandServices(this IServiceCollection sc)
    {
        sc.AddSingleton<ISenseHatClient, SenseHatClient>();
        sc.AddSingleton<IColorFactory, ColorFactory>();
        sc.AddSingleton<ISensHatTextMap, SensHatTextMap>();

        sc.AddSingleton<ClearDisplayCommand>();
        sc.AddSingleton<CurrentSensorValuesCommand>();
        sc.AddSingleton<FillDisplayCommand>();
        sc.AddSingleton<PollSensorsCommand>();
        sc.AddSingleton<RandomFillCommand>();

        return sc;
    }
}