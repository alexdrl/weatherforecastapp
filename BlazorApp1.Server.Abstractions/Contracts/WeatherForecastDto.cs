﻿namespace BlazorApp1.Server.Abstractions.Contracts;

public class WeatherForecastDto
{
    public DateTime Date { get; set; }

    public int TemperatureC { get; set; }

    public string? Summary { get; set; }

    public int TemperatureF { get; set; }
}