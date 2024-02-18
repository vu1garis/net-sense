using System.Collections.Immutable;
using System.Drawing;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatLib.Implementation;

internal sealed class SenseHatClient : ISenseHatClient
{
    private static UnitsNet.Pressure s_defaultSeaLevelPressure = WeatherHelper.MeanSeaLevel;

    private bool _disposed;

    private Lazy<SenseHat> _sh = new Lazy<SenseHat>(() => new SenseHat());

    public SenseHat Hat => _sh.Value;

    public async Task Clear() 
        => await Fill(Color.Empty);

    public async Task Fill(Color[] colors) 
        => await Task.Run(() => Hat.Write(new ReadOnlySpan<Color>(colors)))
                     .ConfigureAwait(false);
    
    public async Task Fill(Color color) 
        => await Task.Run(() => Hat.Fill(color))
                     .ConfigureAwait(false);

    public async Task<SensorReadings> ReadSensors()
    {
        return await Task.Run(() =>
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
        }).ConfigureAwait(false);
    }

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
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

    #endregion
}