namespace React_Redux_ASPdotNET_API.Server.Models
{
    /// <summary>
    /// Boilerplate Weather Forecast Model
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Date of the Weather Forecast
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Temperature in Celsius
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Temperature in Fahrenheit
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// 
        /// </summary>
        public string? Summary { get; set; }
    }
}
