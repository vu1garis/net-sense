using System;
using System.CommandLine;
using SenseHatCli.Implementaiton;

namespace SenseHatCli;

class Program
{
    static int Main(string[] args)
    {
        var rootCommand = new RootCommand("SenseHat CLI");

        using var clearDisplayCommand = new ClearDisplayCommand();
        using var currentSensorValuesCommand = new CurrentSensorValuesCommand();
        using var fillCommand = new FillDisplayCommand();

        rootCommand.AddCommand(clearDisplayCommand);
        rootCommand.AddCommand(currentSensorValuesCommand);
        rootCommand.AddCommand(fillCommand);
        
        return rootCommand.Invoke(args);
    }
}