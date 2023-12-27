using System.CommandLine;
using System.Threading;

namespace SenseHatCli.Implementaiton;

internal sealed class RandomFillCommand : SenseHatCommand
{
    private readonly IColorFactory _colorFactory;

    public RandomFillCommand(ISenseHatClient client, IColorFactory colorFactory)
        : base("random", "fill the sensor display with random RGB colours", client)
    {
        _colorFactory = colorFactory ?? throw new ArgumentNullException(nameof(colorFactory));
    }

    protected override void Configure()
    {
        var loopOption = new Option<bool>(
            name: "--loop",
            description: "Loop if true; otherwise false (default)",
            getDefaultValue: () => false);

        var intervalOption = new Option<int>(
            name: "--interval-ms",
            description: "Polling interval in milliseconds",
            getDefaultValue: () => 1000);

        this.Add(loopOption);
        this.Add(intervalOption);

        this.SetHandler((loop, interval) => 
        {
            while (true)
            {
                var colors = _colorFactory.GetRandomColors(64);

                Client.Fill(colors);

                if (!loop)
                {
                    break;
                }

                Thread.Sleep(interval);
            }
            
        }, loopOption, intervalOption);
    }
}