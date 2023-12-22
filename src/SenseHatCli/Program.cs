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
        clearDisplayCommand.Configure();
        rootCommand.AddCommand(clearDisplayCommand);

        using var currentSensorValuesCommand = new CurrentSensorValuesCommand();
        currentSensorValuesCommand.Configure();
        rootCommand.AddCommand(currentSensorValuesCommand);
        
        return rootCommand.Invoke(args);
    }
}