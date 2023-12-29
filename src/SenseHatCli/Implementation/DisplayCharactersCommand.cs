using System.CommandLine;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace SenseHatCli.Implementaiton;

internal sealed class DisplayCharactersCommand : SenseHatCommand
{
    private readonly ISenseHatBitmapFactory _factory;

    public DisplayCharactersCommand(ISenseHatClient client, ISenseHatBitmapFactory factory)
        : base("char", "sequentially display one or more characters", client)
    {
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    protected override void Configure()
    {
        var displayOption = new Option<string>(  
            name: "--display",
            description: "Characters to display",
            getDefaultValue: () => "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

        var fgOption = new Option<string>(  
            name: "--foreground",
            description: "Foreground System.Drawing.Color name (default Red)",
            getDefaultValue: () => "Red");

        var bgOption = new Option<string>(  
            name: "--background",
            description: "Background System.Drawing.Color name (default Black)",
            getDefaultValue: () => "Black");

        var loopOption = new Option<bool>(
            name: "--loop",
            description: "Loop if true; otherwise false (default)",
            getDefaultValue: () => false);

        var intervalOption = new Option<int>(
            name: "--interval-ms",
            description: "Pause between characters in milliseconds",
            getDefaultValue: () => 1000);

        this.Add(displayOption);
        this.Add(fgOption);
        this.Add(bgOption);
        this.Add(loopOption);
        this.Add(intervalOption);

        this.SetHandler((display, fgName, bgName, loop, interval) => 
        {
            while (true)
            {
                var cache = new Dictionary<char, Color[]>();

                foreach (var c in display)
                {
                    if (cache.ContainsKey(c))
                    {
                        Client.Fill(cache[c]);
                    }
                    else
                    {
                        var bm = _factory.GetBitMap(c) ?? throw new InvalidOperationException($"Character {c} not currently supported");

                        var fg = Color.FromName(fgName);

                        var bg = Color.FromName(bgName);

                        var frame = bm.Color(foreground: fg, background: bg);

                        Client.Fill(frame);

                        if (loop)
                        {
                            cache[c] = frame;
                        }
                    }

                    Thread.Sleep(interval);
                }

                if (!loop)
                {
                    break;
                }
            }
            
        }, displayOption, fgOption, bgOption, loopOption, intervalOption);
    }
}