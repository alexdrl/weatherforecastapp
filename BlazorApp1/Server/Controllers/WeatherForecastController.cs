using BlazorApp1.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherDbContext weatherDbContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherDbContext weatherDbContext)
        {
            _logger = logger;
            this.weatherDbContext = weatherDbContext;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return weatherDbContext.Forecast.OrderByDescending(x => x.Date);
        }

        [HttpPost]
        public async Task AddForecast(WeatherForecast weatherForecast)
        {
            await weatherDbContext.Forecast.AddAsync(weatherForecast);

            await weatherDbContext.SaveChangesAsync();
        }
    }
}