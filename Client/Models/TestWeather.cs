using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Client.Models
{
    public class TestWeather
    {
        public DateTime Date { get; set; }
        public int TemperatureF { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
    }
}
