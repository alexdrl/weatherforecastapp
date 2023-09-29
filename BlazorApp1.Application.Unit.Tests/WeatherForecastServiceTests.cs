
using AutoMapper;
using BlazorApp1.Application.Queue;
using BlazorApp1.Domain;
using BlazorApp1.Domain.Abstractions.Repositories;
using BlazorApp1.Server.Abstractions.Contracts;
using FluentAssertions.Execution;

namespace BlazorApp1.Application.Unit.Tests;

public class WeatherForecastServiceTests
{
    [Theory, AutoMoqData]
    public async Task AddForecast_ShouldAddForecastToRepositoryAndQueueBackgroundTask(
        [Frozen] Mock<IWeatherForecastRepository> weatherForecastRepositoryMock,
        [Frozen] Mock<IBackgroundTaskQueue> backgroundTaskQueueMock,
        [Frozen] Mock<IMapper> mapperMock,
        WeatherForecastDto weatherForecastDto,
        WeatherForecast weatherForecast,
        WeatherForecastService sut)
    {
        // Arrange
        mapperMock.Setup(m => m.Map<WeatherForecast>(weatherForecastDto)).Returns(weatherForecast);

        // Act
        await sut.AddForecast(weatherForecastDto);

        // Assert
        weatherForecastRepositoryMock.Verify(r => r.AddWeatherForecast(weatherForecast), Times.Once);
        backgroundTaskQueueMock.Verify(q => q.QueueBackgroundWorkItemAsync(It.IsAny<Func<IServiceProvider, CancellationToken, ValueTask>>()), Times.Once);
    }

    [Theory, AutoMoqData]
    public async Task ProcessWeatherSummary_WhenExistingForecast_ShouldUpdateSummaryToRepository(
       [Frozen] Mock<IWeatherForecastRepository> weatherForecastRepositoryMock,
       [Frozen] Mock<IWeatherForecastSummaryRepository> weatherForecastSummaryRepositoryMock,
       WeatherForecast[] weatherForecasts,
       WeatherForecastSummary weatherForecastSummary,
       WeatherForecastService sut)
    {
        // Arrange
        weatherForecastRepositoryMock.Setup(r => r.GetAllForecasts()).ReturnsAsync(weatherForecasts);
        weatherForecastSummaryRepositoryMock.Setup(r => r.GetForecastSummaryByDate(It.IsAny<DateOnly>())).ReturnsAsync(weatherForecastSummary);

        // Act
        await sut.ProcessWeatherSummary(weatherForecasts.First(), CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();
        
        weatherForecastSummaryRepositoryMock.Verify(r => r.GetForecastSummaryByDate(DateOnly.FromDateTime(weatherForecasts.First().Date)), Times.Once);
        weatherForecastSummaryRepositoryMock.Verify(r => r.UpdateWeatherForecastSummary(It.IsAny<WeatherForecastSummary>()), Times.Once);
        weatherForecastSummaryRepositoryMock.VerifyNoOtherCalls();
    }

    [Theory, AutoMoqData]
    public async Task ProcessWeatherSummary_WhenNoExistingForecast_ShouldAddSummaryToRepository(
      [Frozen] Mock<IWeatherForecastRepository> weatherForecastRepositoryMock,
      [Frozen] Mock<IWeatherForecastSummaryRepository> weatherForecastSummaryRepositoryMock,
      List<WeatherForecast> weatherForecasts,
      WeatherForecast weatherForecast,
      WeatherForecastService sut)
    {
        // Arrange
        weatherForecast.Date = weatherForecasts.First().Date;
        weatherForecasts.Add(weatherForecast);

        weatherForecastRepositoryMock.Setup(r => r.GetAllForecasts()).ReturnsAsync(weatherForecasts);
        weatherForecastSummaryRepositoryMock.Setup(r => r.GetForecastSummaryByDate(It.IsAny<DateOnly>())).ReturnsAsync((WeatherForecastSummary)null);

        // Act
        await sut.ProcessWeatherSummary(weatherForecast, CancellationToken.None);

        // Assert
        using var scope = new AssertionScope();

        weatherForecastSummaryRepositoryMock.Verify(r => r.GetForecastSummaryByDate(DateOnly.FromDateTime(weatherForecast.Date)), Times.Once);

        var sameDateWeatherforecasts = weatherForecasts.Where(x => DateOnly.FromDateTime(x.Date) == DateOnly.FromDateTime(weatherForecast.Date));
        int tempAcg = (int)Math.Round(sameDateWeatherforecasts.Average(x => x.TemperatureC), 0);

        weatherForecastSummaryRepositoryMock.Verify(r => r.AddWeatherForecastSummary(It.Is<WeatherForecastSummary>(s => s.Date == DateOnly.FromDateTime(weatherForecast.Date) && s.TemperatureC == tempAcg)), Times.Once);
        weatherForecastSummaryRepositoryMock.VerifyNoOtherCalls();
    }
}