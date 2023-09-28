using BlazorApp1.Application.Queue;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorApp1.Application;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddWeatherForecastApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IBackgroundTaskQueue>(_ =>
        {
            if (!int.TryParse(configuration["QueueCapacity"],
                out int queueCapacity))
            {
                queueCapacity = 100;
            }

            return new DefaultBackgroundTaskQueue(queueCapacity);
        });
        services.AddScoped<IWeatherForecastService, WeatherForecastService>();

        return services;
    }
}
