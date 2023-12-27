using System.CommandLine;
using System.Drawing;
using System.Threading;

namespace SenseHatCli.Implementaiton;

internal sealed class DisplayCharactersCommand : SenseHatCommand
{
    private readonly ISenseHatTextMap _textMap;

    public DisplayCharactersCommand(ISenseHatClient client, ISenseHatTextMap textMap)
        : base("char", "sequentially display one or more characters", client)
    {
        _textMap = textMap ?? throw new ArgumentNullException(nameof(textMap));
    }

    protected override void Configure()
    {
        var displayOption = new Option<string>(  
            name: "--display",
            description: "Characters to display",
            getDefaultValue: () => "127");

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
                foreach (var c in display)
                {
                    var bm = _textMap.GetBitMap(c) ?? throw new InvalidOperationException($"Character {c} not currently supported");

                    var fg = Color.FromName(fgName);

                    var bg = Color.FromName(bgName);

                    var frame = bm.Color(foreground: fg, background: bg);

                    Client.Fill(frame);

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