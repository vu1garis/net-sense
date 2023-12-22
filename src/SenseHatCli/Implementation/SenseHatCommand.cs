using System.CommandLine;
using System.Drawing;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal abstract class SenseHatCommand : Command, IDisposable
{
    private bool _disposed;

    private SenseHat? _sh = new SenseHat();

    protected SenseHatCommand(string name, string description)
        : base(name, description)
    {
    }

    protected SenseHat? Hat => _sh;

    public abstract void Configure();

    protected void Clear()
    {
        _sh?.Fill(Color.Black);
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
                    if (_sh != null)
                    {
                        _sh.Dispose();
                        _sh = null;
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