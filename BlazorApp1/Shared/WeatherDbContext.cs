using BlazorApp1.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Shared
{
    public class WeatherDbContext : DbContext
    {
        public WeatherDbContext(DbContextOptions<WeatherDbContext> options)
            : base(options) { }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseSqlite("Data Source=Local.sqlite");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherForecast>().HasData(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id = index,
                Date = new DateTime(2022, 1, 1).AddDays(index),
                TemperatureC = 4 + index,
                Summary = Summaries[1 + index]
            })
            .ToArray());
        }

        public DbSet<WeatherForecast> Forecast { get; set; }

    }
}
