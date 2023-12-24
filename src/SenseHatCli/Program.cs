using System;
using System.CommandLine;

using Microsoft.Extensions.DependencyInjection;

using SenseHatCli.Implementaiton;

namespace SenseHatCli;

class Program
{
    static int Main(string[] args)
    {

        var sc = new ServiceCollection();

        sc.AddSingleton<ISenseHatClient, SenseHatClient>();
        sc.AddSingleton<ClearDisplayCommand>();
        sc.AddSingleton<CurrentSensorValuesCommand>();
        sc.AddSingleton<FillDisplayCommand>();
        sc.AddSingleton<PollSensorsCommand>();

        using var sp = sc.BuildServiceProvider();

        var rootCommand = new RootCommand("SenseHat CLI");

        rootCommand.AddCommand(sp.GetRequiredService<ClearDisplayCommand>());
        rootCommand.AddCommand(sp.GetRequiredService<CurrentSensorValuesCommand>());
        rootCommand.AddCommand(sp.GetRequiredService<FillDisplayCommand>());
        rootCommand.AddCommand(sp.GetRequiredService<PollSensorsCommand>());
        
        return rootCommand.Invoke(args);
    }
}