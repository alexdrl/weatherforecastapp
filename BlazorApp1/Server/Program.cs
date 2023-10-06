using BlazorApp1.Application;
using BlazorApp1.Data;
using BlazorApp1.Server.Abstractions.Contracts.JsonContext;
using BlazorApp1.Server.BackgroundServices;
using BlazorApp1.Server.Profiles;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.AddContext<WeatherForecastDtoJsonContext>();
    });

builder.Services.AddRazorPages();

builder.Services.AddWeatherForecastDataLayer();
builder.Services.AddWeatherForecastApplicationLayer(builder.Configuration);

builder.Services.AddAutoMapper(typeof(WeatherForecastProfile).Assembly);

builder.Services.AddHostedService<QueuedHostedService>();

var app = builder.Build();

await InitializeDatabase(app);
InitializeAutoMapper(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

static async Task InitializeDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
    await db.Database.MigrateAsync();
}

static void InitializeAutoMapper(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var mapperConfiguration = scope.ServiceProvider.GetRequiredService<AutoMapper.IConfigurationProvider>();
    mapperConfiguration.AssertConfigurationIsValid();
    mapperConfiguration.CompileMappings();
}

public partial class Program
{ }