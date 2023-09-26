using BlazorApp1.Data.Abstractions.Repositories;
using BlazorApp1.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastRepository _weatherForecastRepository;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastRepository weatherForecastRepository)
        {
            _logger = logger;
            _weatherForecastRepository = weatherForecastRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return await _weatherForecastRepository.GetAllForecasts();
        }

        [HttpPost]
        public async Task AddForecast(WeatherForecast weatherForecast)
        {
            await _weatherForecastRepository.AddWeatherForecast(weatherForecast);
        }
    }
}