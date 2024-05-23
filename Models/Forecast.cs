using System.Text.Json.Serialization;

namespace WeatherUI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Forecast
    {
        [JsonPropertyName("forecastday")]
        public List<Forecastday> Forecastday { get; set; }
    }


}
