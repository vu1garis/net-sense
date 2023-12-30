using System.Threading;

namespace SenseHatCli.Commands;

internal sealed class PollSensorsCommand : SenseHatCommand
{
    public PollSensorsCommand(ISenseHatClient client)
        : base("poll", "poll the sensor values and write to the console", client)
    {
    }

    protected override void Configure()
    {
        var intervalOption = new Option<int>(
            name: "--interval-ms",
            description: "Polling interval in milliseconds",
            getDefaultValue: () => 5000);

        this.Add(intervalOption);

        this.SetHandler((interval) => 
        {
            while (true)
            {
                var current = Client.ReadSensors();

                Console.WriteLine(current.ToString());

                Thread.Sleep(interval);
            }
        }, intervalOption);
    }
}