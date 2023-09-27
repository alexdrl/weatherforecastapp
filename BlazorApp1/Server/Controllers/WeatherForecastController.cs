using AutoMapper;
using BlazorApp1.Data.Abstractions.Repositories;
using BlazorApp1.Domain;
using BlazorApp1.Server.Abstractions.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly IMapper _mapper;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastRepository weatherForecastRepository, IMapper mapper)
        {
            _logger = logger;
            _weatherForecastRepository = weatherForecastRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecastDto>> Get()
        {
            var forecasts = await _weatherForecastRepository.GetAllForecasts();

            return _mapper.Map<IEnumerable<WeatherForecastDto>>(forecasts);
        }

        [HttpPost]
        public async Task AddForecast(WeatherForecastDto weatherForecast)
        {
            var forecast = _mapper.Map<WeatherForecast>(weatherForecast);

            await _weatherForecastRepository.AddWeatherForecast(forecast);
        }
    }
}