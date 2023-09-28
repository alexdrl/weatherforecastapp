using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorApp1.Domain;

public record WeatherForecastSummary([property: Key][property: DatabaseGenerated(DatabaseGeneratedOption.Identity)] int Id, DateOnly Date, int TemperatureC)
{
    public int TemperatureF => 32 + (int)Math.Round(TemperatureC / 0.5556, 0);
}