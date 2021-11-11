using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AppInsightsDemo.Model
{
    public class Forecasts
    {
        public List<Forecast> Root { get; set; }
    }

    public class Forecast
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("temperatureC")]
        public int TemperatureC { get; set; }

        [JsonPropertyName("temperatureF")]
        public int TemperatureF { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }
    }
}
