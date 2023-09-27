using AutoMapper;
using BlazorApp1.Data;
using BlazorApp1.Domain;
using BlazorApp1.Server.Abstractions.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace BlazorApp1.Server.Integration.Tests
{
    public class WeatherForecastControllerTests /*: IClassFixture<ApiWebApplicationFactory>*/
    {
        private readonly ApiWebApplicationFactory _factory;
        private readonly HttpClient _client;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
         };
        private readonly IMapper? _mapper;

        public WeatherForecastControllerTests()
        {
            _factory = new ApiWebApplicationFactory();
            _client = _factory.CreateClient();

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
                dbContext.Database.EnsureCreated();
                _mapper = scope.ServiceProvider.GetService<IMapper>();
            }
        }

        [Fact]
        public async Task Get_ReturnsWeatherForecasts()
        {
            // Arrange
            var databaseForecasts = new WeatherForecast[]
            {
                new WeatherForecast { Date = DateTime.Now.AddDays(-1), TemperatureC = 20, Summary = "Sunny" },
                new WeatherForecast { Date = DateTime.Now, TemperatureC = 25, Summary = "Cloudy" },
                new WeatherForecast { Date = DateTime.Now.AddDays(1), TemperatureC = 30, Summary = "Rainy" }
            };

            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
                dbContext.Forecast.AddRange(databaseForecasts);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/WeatherForecast");
            response.EnsureSuccessStatusCode();
            var actualForecasts = await response.Content.ReadFromJsonAsync<IEnumerable<WeatherForecastDto>>();

            // Assert
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();

                var expectedForecasts = _mapper?.Map<IEnumerable<WeatherForecastDto>>(dbContext.Forecast);
                actualForecasts.Should().BeEquivalentTo(expectedForecasts);
            }
        }

        [Fact]
        public async Task Post_AddsWeatherForecast()
        {
            // Arrange
            var newForecast = new WeatherForecastDto { Date = DateTime.Now.AddDays(2), TemperatureC = 35, Summary = "Hot", TemperatureF = 95 };

            // Act
            var response = await _client.PostAsJsonAsync("/WeatherForecast", newForecast);
            response.EnsureSuccessStatusCode();

            // Assert
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
                var actualForecast = await dbContext.Forecast.OrderBy(x => x.Id).LastAsync();
                newForecast.Should().BeEquivalentTo(actualForecast, x => x.Excluding(y => y.Id));
            }
        }
    }
}
