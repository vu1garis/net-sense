using System.Drawing;
using Microsoft.Extensions.Logging;

namespace SenseHatCli.Commands;

internal sealed class FillDisplayCommand : SenseHatCommand
{
    public FillDisplayCommand(ILogger<FillDisplayCommand> logger, ISenseHatClient client)
        : base(logger, "fill", "fill the sensor display", client)
    {
    }

    protected override void Configure()
    {
        var colourOption = new Option<string>(
            name: "--colour",
            description: "System.Drawing.Color name (default Red)",
            getDefaultValue: () => "Red");

        this.Add(colourOption);

        this.SetHandler((colorName) =>
        {
            var c = Color.FromName(colorName);
            
            Client.Fill(c);
        }, colourOption);
    }
}