using System.CommandLine;
using System.Drawing;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal abstract class SenseHatCommand : Command, IDisposable
{
    private static UnitsNet.Pressure s_defaultSeaLevelPressure = WeatherHelper.MeanSeaLevel;

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

    protected SensorReadings ReadSensors()
    {
        var t1 = Hat.Temperature;
        var p = Hat.Pressure;

        return new SensorReadings
        {
            Temp1 = t1,
            Temp2 = Hat.Temperature2,
            Pressure = p,
            Humidity = Hat.Humidity,
            Acceleration = Hat.Acceleration,
            AngularRate = Hat.AngularRate,
            MagneticInduction = Hat.MagneticInduction,
            Altitude = WeatherHelper.CalculateAltitude(p, s_defaultSeaLevelPressure, t1),
            HoldingButton = Hat.HoldingButton,
            HoldingUp = Hat.HoldingUp,
            HoldingDown = Hat.HoldingDown,
            HoldingLeft = Hat.HoldingLeft,
            HoldingRight = Hat.HoldingRight
        };
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