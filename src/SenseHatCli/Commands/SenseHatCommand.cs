namespace SenseHatCli.Commands;

internal abstract class SenseHatCommand : Command
{
    private readonly ISenseHatClient _client;

    protected SenseHatCommand(string name, string description, ISenseHatClient client)
        : base(name, description)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));

        Configure();
    }

    protected ISenseHatClient Client => _client;

    protected abstract void Configure();
}