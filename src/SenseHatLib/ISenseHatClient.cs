using System;
using System.Drawing;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatLib;

public interface ISenseHatClient : IDisposable
{
    SenseHat Hat { get; }

    Task Clear();

    Task Fill(Color color); 
    
    Task Fill(Color[] colors);

    Task<SensorReadings> ReadSensors();
}