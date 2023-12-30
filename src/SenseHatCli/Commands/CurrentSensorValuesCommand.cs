namespace SenseHatCli.Commands;

internal sealed class CurrentSensorValuesCommand : SenseHatCommand
{
    public CurrentSensorValuesCommand(ISenseHatClient client)
        : base("current", "display the current sensor values to the console", client)
    {
    }

    protected override void Configure()
    {
        this.SetHandler(() => 
        {
            var current = Client.ReadSensors();

            Console.WriteLine(current.ToString());
        });
    }
}