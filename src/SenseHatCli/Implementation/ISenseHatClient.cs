using System;
using System.Drawing;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal interface ISenseHatClient : IDisposable
{
    SenseHat Hat { get; }

    void Clear();

    void Fill(Color color); 
    
    SensorReadings ReadSensors();
}