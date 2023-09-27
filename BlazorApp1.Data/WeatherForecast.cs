using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Domain
{
    public record WeatherForecast([property: Key][property: DatabaseGenerated(DatabaseGeneratedOption.Identity)] int Id, DateTime Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)Math.Round(TemperatureC / 0.5556, 0);
    }
}