using BlazorApp1.Data.Abstractions.Repositories;
using BlazorApp1.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Data.Repositories;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly WeatherDbContext _weatherDbContext;

    public WeatherForecastRepository(WeatherDbContext weatherDbContext)
    {
        _weatherDbContext = weatherDbContext;
    }

    public async Task AddWeatherForecast(WeatherForecast weatherForecast)
    {
        await _weatherDbContext.Forecast.AddAsync(weatherForecast);

        await _weatherDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<WeatherForecast>> GetAllForecasts()
    {
        return await _weatherDbContext.Forecast.OrderByDescending(x => x.Date).ToListAsync();
    }
}
