using BlazorApp1.Domain;
using BlazorApp1.Domain.Abstractions.Repositories;
using BlazorApp1.Server.Abstractions.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastSummaryController : ControllerBase
{
    private readonly ILogger<WeatherForecastSummaryController> _logger;
    private readonly IWeatherForecastSummaryRepository _weatherForecastSummaryRepository;

    public WeatherForecastSummaryController(ILogger<WeatherForecastSummaryController> logger,
        IWeatherForecastSummaryRepository weatherForecastSummaryRepository)
    {
        _logger = logger;
        _weatherForecastSummaryRepository = weatherForecastSummaryRepository;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecastSummaryDto>> Get()
    {
        var forecastSummaries = await _weatherForecastSummaryRepository.GetAllForecastSummaries();

        return forecastSummaries.Select(x => MapToDto(x));
    }

    private static WeatherForecastSummaryDto MapToDto(WeatherForecastSummary x)
    {
        return new WeatherForecastSummaryDto()
        {
            Date = x.Date,
            TemperatureC = x.TemperatureC,
            TemperatureF = x.TemperatureF
        };
    }
}