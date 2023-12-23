using System.CommandLine;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal sealed class CurrentSensorValuesCommand : SenseHatCommand
{
    public CurrentSensorValuesCommand()
        : base("current", "display the current sensor values to the console")
    {
    }

    public override void Configure()
    {
        this.SetHandler(() => 
        {
            var current = this.ReadSensors();

            Console.WriteLine(current.ToString());
        });
    }
}