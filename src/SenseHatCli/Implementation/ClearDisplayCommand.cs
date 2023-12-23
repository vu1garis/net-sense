using System.CommandLine;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal sealed class ClearDisplayCommand : SenseHatCommand
{
    public ClearDisplayCommand()
        : base("clear", "clear the sensor display")
    {
    }

    protected override void Configure()
    {
        this.SetHandler(() => this.Clear());
    }
}