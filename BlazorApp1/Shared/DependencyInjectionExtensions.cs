using BlazorApp1.Data;
using BlazorApp1.Data.Abstractions.Repositories;
using BlazorApp1.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddWeatherForecastDataLayer(this IServiceCollection services)
        {
            services.AddDbContext<WeatherDbContext>();
            services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

            return services;
        }
    }
}
