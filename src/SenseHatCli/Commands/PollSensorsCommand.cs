using System.Threading;
using Microsoft.Extensions.Logging;

namespace SenseHatCli.Commands;

internal sealed class PollSensorsCommand : SenseHatCommand
{
    public PollSensorsCommand(ILogger<PollSensorsCommand> logger, ISenseHatClient client)
        : base(logger, "poll", "poll the sensor values and write to the console", client)
    {
    }

    protected override void Configure()
    {
        var intervalOption = new Option<int>(
            name: "--interval",
            description: "Polling interval in milliseconds",
            getDefaultValue: () => 5000);

        this.Add(intervalOption);

        this.SetHandler( interval => 
        {
            while (true)
            {
                var current = Client.ReadSensors();

                Logger.LogInformation(current.ToString());

                Task.Delay(interval).Wait();
            }
        }, intervalOption);
    }
}