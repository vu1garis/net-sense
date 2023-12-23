using System.CommandLine;
using System.Drawing;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal abstract class SenseHatCommand : Command, IDisposable
{
    private bool _disposed;

    private Lazy<SenseHat> _sh = new Lazy<SenseHat>(() => new SenseHat());

    protected SenseHatCommand(string name, string description)
        : base(name, description)
    {
        Configure();
    }

    protected SenseHat Hat => _sh.Value;

    public abstract void Configure();

    protected void Clear()
    {
        Hat.Fill(Color.Black);
    }

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            try
            {
                if (disposing)
                {
                    if (_sh.IsValueCreated)
                    {
                        _sh.Value.Dispose();
                    }
                }
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}