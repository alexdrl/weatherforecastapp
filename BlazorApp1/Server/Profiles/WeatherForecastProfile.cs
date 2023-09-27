using AutoMapper;
using BlazorApp1.Domain;
using BlazorApp1.Server.Abstractions.Contracts;

namespace BlazorApp1.Server.Profiles;

public class WeatherForecastProfile : Profile
{
    public WeatherForecastProfile()
    {
        CreateMap<WeatherForecastDto, WeatherForecast>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ReverseMap();
    }
}
