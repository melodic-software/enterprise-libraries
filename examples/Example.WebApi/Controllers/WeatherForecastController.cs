using Enterprise.Domain.Events.Model.Abstract;
using Enterprise.Domain.Events.Raising.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Example.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] _summaries =
    [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ];

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IRaiseQueuedDomainEvents _queuedEventRaiser;
    private readonly IRaiseDomainEvents _eventRaiser;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IRaiseQueuedDomainEvents queuedEventRaiser, IRaiseDomainEvents eventRaiser)
    {
        _logger = logger;
        _queuedEventRaiser = queuedEventRaiser;
        _eventRaiser = eventRaiser;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> GetAsync()
    {
        var domainEvent = new TestEvent();

        await _eventRaiser.RaiseAsync(domainEvent);

        await _queuedEventRaiser.RaiseAsync(domainEvent);

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries[Random.Shared.Next(_summaries.Length)]
            })
            .ToArray();
    }

    public class TestEvent : DomainEvent
    {
        
    }
}
