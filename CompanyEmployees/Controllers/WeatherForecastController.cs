using LoggingService;
using Microsoft.AspNetCore.Mvc;

namespace CompanyEmployees.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController(ILoggerManager logger) : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILoggerManager _logger = logger;

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        _logger.LogDebug("This is a debug message.");
        _logger.LogInformation("This is an information message.");
        _logger.LogWarning("This is a warning message.");
        _logger.LogError("This is an error message.");

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}