namespace BlazorApp1.Domain.Tests;

public class WeatherForecastTests
{
    [Fact]
    public void TemperatureF_ConversionIsCorrect()
    {
        // Arrange
        var weatherForecast = new WeatherForecast
        {
            TemperatureC = 25
        };

        // Act
        int temperatureF = weatherForecast.TemperatureF;

        // Assert
        temperatureF.Should().Be(77);
    }

    [Fact]
    public void TemperatureF_ConversionIsCorrectWithNegativeCelsius()
    {
        // Arrange
        var weatherForecast = new WeatherForecast
        {
            TemperatureC = -10
        };

        // Act
        int temperatureF = weatherForecast.TemperatureF;

        // Assert
        temperatureF.Should().Be(14);
    }
}