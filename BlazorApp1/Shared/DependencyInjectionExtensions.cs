using BlazorApp1.Data.Abstractions.Repositories;
using BlazorApp1.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1.Data;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddWeatherForecastDataLayer(this IServiceCollection services)
    {
        services.AddDbContext<WeatherDbContext>();
        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}
