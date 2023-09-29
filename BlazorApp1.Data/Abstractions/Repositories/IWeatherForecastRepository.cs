using BlazorApp1.Domain;

namespace BlazorApp1.Domain.Abstractions.Repositories;

public interface IWeatherForecastRepository
{
    public Task<IEnumerable<WeatherForecast>> GetAllForecasts();

    public Task AddWeatherForecast(WeatherForecast weatherForecast);
}
