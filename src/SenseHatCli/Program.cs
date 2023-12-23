using System;
using System.CommandLine;
using SenseHatCli.Implementaiton;

namespace SenseHatCli;

class Program
{
    static int Main(string[] args)
    {
        using var clearDisplayCommand = new ClearDisplayCommand();
        using var currentSensorValuesCommand = new CurrentSensorValuesCommand();
        using var fillCommand = new FillDisplayCommand();
        using var pollCommand = new PollSensorsCommand();

        var rootCommand = new RootCommand("SenseHat CLI");

        rootCommand.AddCommand(clearDisplayCommand);
        rootCommand.AddCommand(currentSensorValuesCommand);
        rootCommand.AddCommand(fillCommand);
        rootCommand.AddCommand(pollCommand);
        
        return rootCommand.Invoke(args);
    }
}