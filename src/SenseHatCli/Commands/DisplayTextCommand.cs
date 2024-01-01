using System.Drawing;
using System.Threading;
using SenseHatLib;

namespace SenseHatCli.Commands;

internal sealed class DisplayTextCommand : SenseHatCommand
{
    private readonly ISenseHatDisplay _display;

    public DisplayTextCommand(ISenseHatDisplay display, ISenseHatClient client)
        : base("display", "sequentially display one or more characters", client)
    {
        _display = display ?? throw new ArgumentNullException(nameof(display));
    }

    protected override void Configure()
    {
        var displayOption = new Option<string>(  
            name: "--text",
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

        var scrollOption = new Option<bool>(
            name: "--scroll",
            description: "Scroll if true; otherwise false (default)",
            getDefaultValue: () => false);

        var intervalOption = new Option<int>(
            name: "--interval",
            description: "Pause between characters in milliseconds",
            getDefaultValue: () => 1000);

        this.Add(displayOption); 
        this.Add(fgOption);
        this.Add(bgOption);
        this.Add(loopOption);
        this.Add(scrollOption);
        this.Add(intervalOption);

        this.SetHandler((display, fgName, bgName, loop, scroll, interval) => 
        {
            var fg = Color.FromName(fgName);

            var bg = Color.FromName(bgName);

            _display.DisplayText(
                text: display,
                foreground: fg,
                background: bg,
                loop: loop,
                scroll: scroll,
                delay: interval);
                
        }, displayOption, fgOption, bgOption, loopOption, scrollOption, intervalOption);
    }
}