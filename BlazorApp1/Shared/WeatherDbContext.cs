using Microsoft.EntityFrameworkCore;

namespace BlazorApp1.Shared
{
    public class WeatherDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
            {
                return;
            }

            optionsBuilder.UseSqlite("Data Source=Local.sqlite");
        }

        public DbSet<WeatherForecast> Forecast { get; set; }

    }
}
