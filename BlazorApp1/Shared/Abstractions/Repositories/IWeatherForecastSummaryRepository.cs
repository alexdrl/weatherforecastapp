using BlazorApp1.Domain;

namespace BlazorApp1.Data.Abstractions.Repositories;

public interface IWeatherForecastSummaryRepository
{
    public Task<IEnumerable<WeatherForecastSummary>> GetAllForecastSummaries();

    public Task AddWeatherForecastSummary(WeatherForecastSummary weatherForecastSummary);
    Task<WeatherForecastSummary?> GetForecastSummaryByDate(DateOnly date);
    Task UpdateWeatherForecastSummary(WeatherForecastSummary weatherForecastSummary);
}
