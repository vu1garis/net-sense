using Microsoft.Extensions.DependencyInjection;
using SenseHatCli.Commands;

namespace SenseHatCli;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSenseHatCommands(this IServiceCollection sc)
    {
        sc.AddSingleton<ClearDisplayCommand>();
        sc.AddSingleton<CurrentSensorValuesCommand>();
        sc.AddSingleton<FillDisplayCommand>();
        sc.AddSingleton<PollSensorsCommand>();
        sc.AddSingleton<RandomFillCommand>();
        sc.AddSingleton<DisplayTextCommand>();
        sc.AddSingleton<FortuneCommand>();

        return sc;
    }
}