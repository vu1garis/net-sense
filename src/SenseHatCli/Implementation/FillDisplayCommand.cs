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
        var rOption = new Option<int>(
            name: "--red",
            description: "Red channel 0 - 255",
            getDefaultValue: () => 0);

        var gOption = new Option<int>(
            name: "--green",
            description: "Green channel 0 - 255",
            getDefaultValue: () => 0);

        var bOption = new Option<int>(
            name: "--blue",
            description: "Blue channel 0 - 255",
            getDefaultValue: () => 0);

        var aOption = new Option<int>(
            name: "--alpha",
            description: "Alpha channel 0 - 255",
            getDefaultValue: () => 0);    

        this.Add(aOption);
        this.Add(rOption);
        this.Add(gOption);
        this.Add(bOption);

        this.SetHandler((a, r, g, b) =>
        {
            var c = Color.FromArgb(alpha: a, red: r, green: g, blue: b);
            
            Client.Fill(c);
        }, aOption, rOption, gOption, bOption);
    }
}