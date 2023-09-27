using AutoMapper;
using BlazorApp1.Data.Abstractions.Repositories;
using BlazorApp1.Domain;
using BlazorApp1.Server.Abstractions.Contracts;
using BlazorApp1.Server.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace BlazorApp1.Server.Unit.Tests;

public class WeatherForecastControllerTests
{
    private readonly Mock<ILogger<WeatherForecastController>> _loggerMock;
    private readonly Mock<IWeatherForecastRepository> _weatherForecastRepositoryMock;
    private readonly IMapper _mapper;

    public WeatherForecastControllerTests()
    {
        _loggerMock = new Mock<ILogger<WeatherForecastController>>();
        _weatherForecastRepositoryMock = new Mock<IWeatherForecastRepository>();

        var services = new ServiceCollection();
        services.AddAutoMapper(typeof(WeatherForecastController).Assembly);
        var servicesCollection = services.BuildServiceProvider();
        _mapper = servicesCollection.GetRequiredService<IMapper>();
    }

    [Fact]
    public async Task Get_ShouldReturnAllForecasts()
    {
        // Arrange
        var forecasts = new List<WeatherForecast>
        {
            new WeatherForecast(default, default, default, null) { Date = DateTime.Today, TemperatureC = 20, Summary = "Sunny" },
            new WeatherForecast(default, default, default, null) { Date = DateTime.Today.AddDays(1), TemperatureC = 15, Summary = "Cloudy" }
        };
        _weatherForecastRepositoryMock.Setup(x => x.GetAllForecasts()).ReturnsAsync(forecasts);

        var controller = new WeatherForecastController(_loggerMock.Object, _weatherForecastRepositoryMock.Object, _mapper);

        // Act
        var result = await controller.Get();

        // Assert
        result.Should().BeEquivalentTo(_mapper.Map<IEnumerable<WeatherForecastDto>>(forecasts));
        _weatherForecastRepositoryMock.Verify(x => x.GetAllForecasts(), Times.Once);
    }

    [Fact]
    public async Task Get_ShouldReturnEmptyListWhenNoForecasts()
    {
        // Arrange
        var forecasts = new List<WeatherForecast>();
        _weatherForecastRepositoryMock.Setup(x => x.GetAllForecasts()).ReturnsAsync(forecasts);

        var controller = new WeatherForecastController(_loggerMock.Object, _weatherForecastRepositoryMock.Object, _mapper);

        // Act
        var result = await controller.Get();

        // Assert
        result.Should().BeEmpty();
        _weatherForecastRepositoryMock.Verify(x => x.GetAllForecasts(), Times.Once);
    }

    [Fact]
    public async Task Get_ShouldReturnExceptionWhenException()
    {
        // Arrange
        var forecasts = new List<WeatherForecast>();
        _weatherForecastRepositoryMock.Setup(x => x.GetAllForecasts()).Throws(new Exception());

        var controller = new WeatherForecastController(_loggerMock.Object, _weatherForecastRepositoryMock.Object, _mapper);

        // Act
        var result = async () => await controller.Get();

        // Assert
        await result.Should().ThrowAsync<Exception>();
        _weatherForecastRepositoryMock.Verify(x => x.GetAllForecasts(), Times.Once);
    }

    [Fact]
    public async Task AddForecast_ShouldCallRepository()
    {
        // Arrange
        var forecast = new WeatherForecast(default, default, default, null) { Date = DateTime.Today, TemperatureC = 25, Summary = "Hot" };
        _weatherForecastRepositoryMock.Setup(x => x.AddWeatherForecast(forecast)).Returns(Task.CompletedTask);

        var controller = new WeatherForecastController(_loggerMock.Object, _weatherForecastRepositoryMock.Object, _mapper);

        // Act
        var forecastDto = new WeatherForecastDto { Date = DateTime.Today, TemperatureC = 25, Summary = "Hot" };

        await controller.AddForecast(forecastDto);

        // Assert
        _weatherForecastRepositoryMock.Verify(x => x.AddWeatherForecast(It.IsAny<WeatherForecast>()), Times.Once);
    }
}
