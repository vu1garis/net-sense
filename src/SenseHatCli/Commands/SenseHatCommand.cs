using Microsoft.Extensions.Logging;

namespace SenseHatCli.Commands;

internal abstract class SenseHatCommand : Command
{
    private readonly ISenseHatClient _client;

    private readonly ILogger _logger;

    protected SenseHatCommand(ILogger logger, string name, string description, ISenseHatClient client)
        : base(name, description)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _client = client ?? throw new ArgumentNullException(nameof(client));

        Configure();
    }

    protected Microsoft.Extensions.Logging.ILogger Logger => _logger;

    protected ISenseHatClient Client => _client;

    protected abstract void Configure();
}