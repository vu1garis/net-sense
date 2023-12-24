using System.CommandLine;

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