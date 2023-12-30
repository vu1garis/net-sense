using System;
using Iot.Device.SenseHat;


namespace SenseHatLib;

public record class SensorReadings
{
    public UnitsNet.Temperature Temp1 { get; init; }
    public UnitsNet.Temperature Temp2 { get; init; }
    public UnitsNet.Pressure Pressure { get; init; }
    public UnitsNet.RelativeHumidity Humidity { get; init; }

    public System.Numerics.Vector3 Acceleration { get; init; }
    public System.Numerics.Vector3 AngularRate { get; init; }
    public System.Numerics.Vector3 MagneticInduction { get; init; }
    
    public UnitsNet.Length Altitude { get; init; }

    public bool HoldingButton { get; init; }
    public bool HoldingUp { get; init; }
    public bool HoldingDown { get; init; }
    public bool HoldingLeft { get; init; }
    public bool HoldingRight { get; init; }
}