using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlazorApp1.Data.Design;

public class WeatherDbContextFactory : IDesignTimeDbContextFactory<WeatherDbContext>
{
    public WeatherDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WeatherDbContext>();
        optionsBuilder.UseSqlite("Data Source=Local.sqlite");

        return new WeatherDbContext(optionsBuilder.Options);
    }
}