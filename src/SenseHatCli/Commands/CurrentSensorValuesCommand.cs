using System.Drawing;

namespace SenseHatCli.Commands;

internal sealed class CurrentSensorValuesCommand : SenseHatCommand
{
    private readonly ISenseHatDisplay _display;

    public CurrentSensorValuesCommand(ISenseHatDisplay display, ISenseHatClient client)
        : base("current", "display the current sensor values to the console", client)
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
            var current = Client.ReadSensors();

            Console.WriteLine(current.ToString());

            var fg = Color.FromName(fgName);

            var bg = Color.FromName(bgName);

            var display = $"Temp1 = {current.Temp1}, Temp2 = {current.Temp2}, Pressure = {current.Pressure}, Humidity = {current.Humidity}";

            _display.DisplayText(
                text: display,
                foreground: fg,
                background: bg,
                loop: loop,
                scroll: true,
                delay: interval);
                
        }, fgOption, bgOption, loopOption, intervalOption);
    }
}