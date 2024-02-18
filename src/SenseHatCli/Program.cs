using System;
using System.CommandLine;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SenseHatLib;
using SenseHatCli.Commands;

namespace SenseHatCli;

class Program
{
    public static async Task<int> Main(string[] args)
    {
        var sc = new ServiceCollection();

        sc.AddLogging(builder => builder.AddConsole());

        sc.AddSenseHatServices();
        sc.AddSenseHatCommands();

        using var sp = sc.BuildServiceProvider();

        var rootCommand = new RootCommand("SenseHat CLI");

        rootCommand.AddCommand(sp.GetRequiredService<ClearDisplayCommand>());
        rootCommand.AddCommand(sp.GetRequiredService<CurrentSensorValuesCommand>());
        rootCommand.AddCommand(sp.GetRequiredService<FillDisplayCommand>());
        rootCommand.AddCommand(sp.GetRequiredService<PollSensorsCommand>());
        rootCommand.AddCommand(sp.GetRequiredService<RandomFillCommand>());
        rootCommand.AddCommand(sp.GetRequiredService<DisplayTextCommand>());
        
        return await rootCommand.InvokeAsync(args);
    }
}