using BlazorApp1.Domain;
using BlazorApp1.Server.Abstractions.Contracts;

namespace BlazorApp1.Application;
public interface IWeatherForecastService
{
    Task AddForecast(WeatherForecastDto weatherForecast, CancellationToken cancellationToken = default);
    ValueTask ProcessSignalRWeatherSummary(CancellationToken cancellationToken);
    ValueTask ProcessWeatherSummary(WeatherForecast forecast, CancellationToken cancellationToken);
}