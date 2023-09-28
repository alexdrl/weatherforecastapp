using BlazorApp1.Data.Abstractions.Repositories;
using BlazorApp1.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1.Data;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddWeatherForecastDataLayer(this IServiceCollection services)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "data", "Local.sqlite");

        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        }

        services.AddDbContext<WeatherDbContext>(optionsBuilder => optionsBuilder.UseSqlite($"Data Source={filePath}"));
        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();
        services.AddScoped<IWeatherForecastSummaryRepository, WeatherForecastSummaryRepository>();

        return services;
    }
}
