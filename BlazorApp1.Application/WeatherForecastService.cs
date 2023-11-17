using AutoMapper;
using BlazorApp1.Application.Queue;
using BlazorApp1.Domain;
using BlazorApp1.Domain.Abstractions.Repositories;
using BlazorApp1.Server.Abstractions.Contracts;
using BlazorApp1.Server.Hubs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlazorApp1.Application;

public class WeatherForecastService : IWeatherForecastService
{
    private readonly IWeatherForecastRepository _weatherForecastRepository;
    private readonly IWeatherForecastSummaryRepository _weatherForecastSummaryRepository;
    private readonly IMapper _mapper;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IHubContext<WeatherForecastSummaryHub> hubContext;
    private readonly ILogger<WeatherForecastService> _logger;

    public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository,
        IWeatherForecastSummaryRepository weatherForecastSummaryRepository,
        IMapper mapper,
        IBackgroundTaskQueue backgroundTaskQueue,
        IHubContext<WeatherForecastSummaryHub> hubContext,
        ILogger<WeatherForecastService> logger)
    {
        _weatherForecastRepository = weatherForecastRepository;
        _weatherForecastSummaryRepository = weatherForecastSummaryRepository;
        _mapper = mapper;
        _backgroundTaskQueue = backgroundTaskQueue;
        this.hubContext = hubContext;
        _logger = logger;
    }

    public async Task AddForecast(WeatherForecastDto weatherForecast, CancellationToken cancellationToken = default)
    {
        var forecast = _mapper.Map<WeatherForecast>(weatherForecast);

        await _weatherForecastRepository.AddWeatherForecast(forecast);

        await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(ProcessWeatherSummary(forecast));
    }

    private static Func<IServiceProvider, CancellationToken, ValueTask> ProcessSignalRWeatherSummary(WeatherForecast forecast)
    {
        return (serviceProvider, cancellationToken) =>
        {
            var service = serviceProvider.GetRequiredService<IWeatherForecastService>();
            return service.ProcessSignalRWeatherSummary(cancellationToken);
        };
    }

    public async ValueTask ProcessSignalRWeatherSummary(CancellationToken cancellationToken)
    {
        //TODO: Move to a new RepoMethod.
        var forecasts = await _weatherForecastSummaryRepository.GetAllForecastSummaries();

        await this.hubContext.Clients.All.SendAsync("ReceiveForecastSummaries", forecasts.ToArray());
    }

    private static Func<IServiceProvider, CancellationToken, ValueTask> ProcessWeatherSummary(WeatherForecast forecast)
    {
        return (serviceProvider, cancellationToken) =>
        {
            var service = serviceProvider.GetRequiredService<IWeatherForecastService>();
            return service.ProcessWeatherSummary(forecast, cancellationToken);
        };
    }

    public async ValueTask ProcessWeatherSummary(WeatherForecast forecast, CancellationToken cancellationToken)
    {
        //TODO: Move to a new RepoMethod.
        var forecasts = await _weatherForecastRepository.GetAllForecasts();
        forecasts = forecasts.Where(dbForecast => DateOnly.FromDateTime(forecast.Date) == DateOnly.FromDateTime(dbForecast.Date));

        var forecastSummary = new WeatherForecastSummary
        {
            Date = DateOnly.FromDateTime(forecast.Date),
            TemperatureC = (int)Math.Round(forecasts.Average(x => x.TemperatureC), 0)
        };

        var existingSummary = await _weatherForecastSummaryRepository.GetForecastSummaryByDate(forecastSummary.Date);
        if (existingSummary != null)
        {
            existingSummary.TemperatureC = forecastSummary.TemperatureC;
            await _weatherForecastSummaryRepository.UpdateWeatherForecastSummary(existingSummary);
        }
        else
        {
            await _weatherForecastSummaryRepository.AddWeatherForecastSummary(forecastSummary);
        }

        await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(ProcessSignalRWeatherSummary(forecast));

        _logger.LogInformation($"Processing ForecastSummary and added {forecast.Date}");
    }
}
