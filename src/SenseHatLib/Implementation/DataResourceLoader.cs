using System.Reflection;
using System.Text;

namespace SenseHatLib.Implementation;

internal class DataResourceLoader
{
    private static readonly Lazy<Assembly> s_Executing = 
        new Lazy<Assembly>(() => Assembly.GetExecutingAssembly());

    public static void Load(string resourcePath, Action<string> add)
    {
        if (string.IsNullOrWhiteSpace(resourcePath))
        {
            throw new ArgumentException(nameof(resourcePath));
        }

        if (add == default)
        {
            throw new ArgumentException(nameof(add));
        }

        using var res = s_Executing.Value.GetManifestResourceStream(resourcePath) ?? throw new InvalidOperationException($"{resourcePath} not found");

        using var sr = new StreamReader(res, Encoding.UTF8) ?? throw new InvalidOperationException("read tmap.xml failed");

        string? line = null;

        while((line = sr.ReadLine()) != null)
        {
            add(line.Trim());
        };
    }

    public static T Load<T>(string resourcePath, Func<StreamReader, T> parser)
    {
        if (string.IsNullOrWhiteSpace(resourcePath))
        {
            throw new ArgumentException(nameof(resourcePath));
        }

        using var res = s_Executing.Value.GetManifestResourceStream(resourcePath) ?? throw new InvalidOperationException($"{resourcePath} not found");

        using var sr = new StreamReader(res, Encoding.UTF8) ?? throw new InvalidOperationException("read tmap.xml failed");

        return parser(sr);
    }
}
