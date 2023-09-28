using BlazorApp1.Data.Abstractions.Repositories;
using BlazorApp1.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data.Repositories;

public class WeatherForecastSummaryRepository : IWeatherForecastSummaryRepository
{
    private readonly WeatherDbContext _weatherDbContext;

    public WeatherForecastSummaryRepository(WeatherDbContext weatherDbContext)
    {
        _weatherDbContext = weatherDbContext;
    }
  
    public async Task AddWeatherForecastSummary(WeatherForecastSummary weatherForecastSummary)
    {
        await _weatherDbContext.ForecastSummaries.AddAsync(weatherForecastSummary);

        await _weatherDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<WeatherForecastSummary>> GetAllForecastSummaries()
    {
        return await _weatherDbContext.ForecastSummaries.OrderByDescending(x => x.Date).ToListAsync();
    }
}
