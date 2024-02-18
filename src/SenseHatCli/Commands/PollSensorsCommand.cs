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
            name: "--interval",
            description: "Polling interval in milliseconds",
            getDefaultValue: () => 5000);

        this.Add(intervalOption);

        this.SetHandler(async (interval) => 
        {
            while (true)
            {
                var current = await Client.ReadSensors().ConfigureAwait(false);

                Console.WriteLine(current.ToString());

                await Task.Delay(interval).ConfigureAwait(false);
            }
        }, intervalOption);
    }
}