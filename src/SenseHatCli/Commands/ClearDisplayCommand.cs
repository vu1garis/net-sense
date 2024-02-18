using Microsoft.Extensions.Logging;

namespace SenseHatCli.Commands;

internal sealed class ClearDisplayCommand : SenseHatCommand
{
    public ClearDisplayCommand(ILogger<ClearDisplayCommand> logger, ISenseHatClient client)
        : base(logger, "clear", "clear the sensor display", client)
    {
    }

    protected override void Configure()
    {
        this.SetHandler(async () => await Client.Clear().ConfigureAwait(false));
    }
}