using System.Text.Json.Serialization;

namespace WeatherUI.Models
{
    /// <summary>
    /// Represents the current weather conditions
    /// </summary>
    public class Current
    {
        [JsonPropertyName("condition")]
        public Condition Condition { get; set; }
    }


}
