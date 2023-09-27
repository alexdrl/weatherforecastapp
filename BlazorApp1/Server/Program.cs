using BlazorApp1.Data;
using BlazorApp1.Server.Profiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddWeatherForecastDataLayer();
builder.Services.AddAutoMapper(typeof(WeatherForecastProfile).Assembly);

var app = builder.Build();

InitializeDatabase(app);
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

static void InitializeDatabase(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<WeatherDbContext>();
    db.Database.Migrate();
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