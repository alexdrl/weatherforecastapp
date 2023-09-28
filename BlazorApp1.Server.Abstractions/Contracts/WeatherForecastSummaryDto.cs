namespace BlazorApp1.Server.Abstractions.Contracts;

public class WeatherForecastSummaryDto
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }
}