using BlazorApp1.Domain;
using BlazorApp1.Server.Abstractions.Contracts;
using Microsoft.AspNetCore.SignalR;

namespace BlazorApp1.Server.Hubs;

public class WeatherForecastSummaryHub : Hub
{
    public async Task SendWeatherforecastSummaries(WeatherForecastSummaryDto[] summaries)
    {
        await Clients.All.SendAsync("ReceiveForecastSummaries", summaries);
    }
}