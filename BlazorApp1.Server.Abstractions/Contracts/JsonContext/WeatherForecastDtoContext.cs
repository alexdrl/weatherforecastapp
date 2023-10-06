using System.Text.Json.Serialization;

namespace BlazorApp1.Server.Abstractions.Contracts.JsonContext;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(WeatherForecastDto))]
[JsonSerializable(typeof(WeatherForecastDto[]))]
[JsonSerializable(typeof(IEnumerable<WeatherForecastDto>))]
[JsonSerializable(typeof(List<WeatherForecastDto>))]
[JsonSerializable(typeof(IEnumerable<WeatherForecastSummaryDto>))]
[JsonSerializable(typeof(List<WeatherForecastSummaryDto>))]
[JsonSerializable(typeof(WeatherForecastSummaryDto))]
[JsonSerializable(typeof(WeatherForecastSummaryDto[]))]
public partial class WeatherForecastDtoJsonContext : JsonSerializerContext
{
}