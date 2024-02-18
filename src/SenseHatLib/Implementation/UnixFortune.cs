namespace SenseHatLib.Implementation;

internal class UnixFortune : IUnixFortune
{
    private readonly Lazy<Queue<string>> _fortunes = new Lazy<Queue<string>>(
        () =>
        {
            var data = new List<string>();

            DataResourceLoader.Load("SenseHatLib.data.v8-fortunes.txt", line => data.Add(line));

            var indexes = Enumerable.Range(0, data.Count -1).ToArray();

            var rndIndex = Random.Shared.GetItems(indexes, data.Count -1);

            var fortunes = new Queue<string>();

            Array.ForEach(rndIndex, i => fortunes.Enqueue(data[i]));

            return fortunes;
        });

    public string? Next()
    {
        if (_fortunes.Value.TryDequeue(out var fortune))
        {
            _fortunes.Value.Enqueue(fortune);
        }

        return fortune;
    }
}
