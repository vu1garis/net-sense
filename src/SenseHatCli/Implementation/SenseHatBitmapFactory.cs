using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace SenseHatCli.Implementaiton;

internal sealed class SenseHatBitmapFactory : ISenseHatBitmapFactory
{
    private const int MAX_PIXELS = 64;

    private readonly Lazy<Dictionary<char,BitArray>> _map = new Lazy<Dictionary<char,BitArray>>(
        () => 
        {
            var assembly = Assembly.GetExecutingAssembly();

            using var res = assembly.GetManifestResourceStream("SenseHatCli.tmap.xml") ?? throw new InvalidOperationException("tmap.xml not found");

            using var sr = new StreamReader(res, Encoding.UTF8) ?? throw new InvalidOperationException("read tmap.xml failed");

            var xml = XElement.Load(sr, LoadOptions.None);

            var map = new Dictionary<char, BitArray>();

            foreach (var e in xml.Elements("c"))
            {
                var keyAttr = e.Attribute("key") ?? throw new InvalidOperationException("Could not find key attribute reading tmap.xml"); 

                if (string.IsNullOrWhiteSpace(keyAttr.Value))
                {
                    throw new InvalidOperationException("Null key attribute value reading tmap.xml");
                }

                var key = char.Parse(keyAttr.Value);

                var arr = e.Value.Where(c => c == '.' || c == '@').Select(c => c == '@' ? true : false).ToArray();

                var bm = new BitArray(arr);

                if (bm.Length != MAX_PIXELS)
                {
                    throw new InvalidOperationException($"Error reading cmap.xml, invalid character arraly length for entry {key}.");
                }

                map.Add(key, bm);
            }

            // add an entry for SPACE
            map.Add(' ', new BitArray(MAX_PIXELS, false));

            return map;
        }
    );

    public Dictionary<char,BitArray> Map => _map.Value; 

    public bool ContainsKey(char character) => Map.ContainsKey(character);

    public BitArray? GetBitMap(char character) => Map.ContainsKey(character) ? Map[character] : default(BitArray);
}