using System;
using System.Drawing;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatLib;

public interface ISenseHatClient : IDisposable
{
    SenseHat Hat { get; }

    void Clear();

    void Fill(Color color); 
    
    void Fill(ReadOnlySpan<Color> colors);

    SensorReadings ReadSensors();
}