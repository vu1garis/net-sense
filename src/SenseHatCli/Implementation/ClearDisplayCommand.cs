using System.CommandLine;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal sealed class ClearDisplayCommand : SenseHatCommand
{
    public ClearDisplayCommand(ISenseHatClient client)
        : base("clear", "clear the sensor display", client)
    {
    }

    protected override void Configure()
    {
        this.SetHandler(() => Client.Clear());
    }
}