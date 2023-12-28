using System.CommandLine;
using System.CommandLine.Binding;

using System.Drawing;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal sealed class FillDisplayCommand : SenseHatCommand
{
    public FillDisplayCommand(ISenseHatClient client)
        : base("fill", "fill the sensor display", client)
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