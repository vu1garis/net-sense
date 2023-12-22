using System.CommandLine;

using Iot.Device.Common;
using Iot.Device.SenseHat;

namespace SenseHatCli.Implementaiton;

internal sealed class CurrentSensorValuesCommand : SenseHatCommand
{
    public CurrentSensorValuesCommand()
        : base("current", "display the current sensor values to the console")
    {
    }

    public override void Configure()
    {
        this.SetHandler(() => 
        {
            // set this to the current sea level pressure in the area for correct altitude readings
            var defaultSeaLevelPressure = WeatherHelper.MeanSeaLevel;

            var tempValue = Hat.Temperature;
            var temp2Value = Hat.Temperature2;
            var preValue = Hat.Pressure;
            var humValue = Hat.Humidity;
            var altValue = WeatherHelper.CalculateAltitude(preValue, defaultSeaLevelPressure, tempValue);

            Console.WriteLine($"Temperature Sensor 1: {tempValue.DegreesCelsius:0.#}\u00B0C");
            Console.WriteLine($"Temperature Sensor 2: {temp2Value.DegreesCelsius:0.#}\u00B0C");
            Console.WriteLine($"Pressure: {preValue.Hectopascals:0.##} hPa");
            Console.WriteLine($"Altitude: {altValue.Meters:0.##} m");
            Console.WriteLine($"Relative humidity: {humValue.Percent:0.#}%");
            Console.WriteLine($"Heat index: {WeatherHelper.CalculateHeatIndex(tempValue, humValue).DegreesCelsius:0.#}\u00B0C");
            Console.WriteLine($"Dew point: {WeatherHelper.CalculateDewPoint(tempValue, humValue).DegreesCelsius:0.#}\u00B0C");
        });
    }
}