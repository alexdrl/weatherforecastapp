using BlazorApp1.Domain;

namespace BlazorApp1.Domain.Abstractions.Repositories;

public interface IWeatherForecastSummaryRepository
{
    public Task<IEnumerable<WeatherForecastSummary>> GetAllForecastSummaries();

    public Task AddWeatherForecastSummary(WeatherForecastSummary weatherForecastSummary);
    Task<WeatherForecastSummary?> GetForecastSummaryByDate(DateOnly date);
    Task UpdateWeatherForecastSummary(WeatherForecastSummary weatherForecastSummary);
}
