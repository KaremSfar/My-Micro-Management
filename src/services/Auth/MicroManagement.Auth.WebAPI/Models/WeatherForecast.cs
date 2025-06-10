namespace MicroManagement.Auth.WebAPI.Models
{
    /// <summary>
    /// Weather Forecast DTO
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Date of the forecast
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Temperature in Celsius
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Temperature in Fareneheit
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// Summary of the weather Forecast
        /// </summary>
        public string? Summary { get; set; }
    }
}