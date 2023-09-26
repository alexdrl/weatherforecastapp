using BlazorApp1.Domain;

namespace BlazorApp1.Data.Abstractions.Repositories
{
    public interface IWeatherForecastRepository
    {
        public Task<IEnumerable<WeatherForecast>> GetAllForecasts();

        public Task AddWeatherForecast(WeatherForecast weatherForecast);
    }
}
