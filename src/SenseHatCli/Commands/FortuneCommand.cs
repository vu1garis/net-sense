using System.Drawing;
using System.Threading;
using Microsoft.Extensions.Logging;
using SenseHatLib;

namespace SenseHatCli.Commands;

internal sealed class FortuneCommand : SenseHatCommand
{
    private readonly ISenseHatDisplay _display;

    public FortuneCommand(ILogger<FortuneCommand> logger, ISenseHatDisplay display, ISenseHatClient client)
        : base(logger, "fortune", "generate a UNIX fortune", client)
    {
        _display = display ?? throw new ArgumentNullException(nameof(display));
    }

    protected override void Configure()
    {
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
            name: "--interval",
            description: "Pause between characters in milliseconds",
            getDefaultValue: () => 1000);

        this.Add(fgOption);
        this.Add(bgOption);
        this.Add(loopOption);
        this.Add(intervalOption);

        this.SetHandler((fgName, bgName, loop, interval) => 
        {
            var fg = Color.FromName(fgName);

            var bg = Color.FromName(bgName);

            _display.DisplayFortune(
                foreground: fg,
                background: bg,
                loop: loop,
                delay: interval);
                
        }, fgOption, bgOption, loopOption, intervalOption);
    }
}
