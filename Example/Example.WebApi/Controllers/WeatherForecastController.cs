using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AVStack.MessageBus.Abstraction;
using Example.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Example.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMessageBusFactory _busFactory;
        
        public WeatherForecastController(IMessageBusFactory busFactory, ILogger<WeatherForecastController> logger)
        {
            _busFactory = busFactory;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var weather = Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray();
            // Publish a weather forecast
            // -----------------------------------------------------------
            
            

            using (var producer = _busFactory.CreateProducer())
            {
                foreach (var item in weather)
                {
                    var message = JsonSerializer.Serialize(item);
                    
                    if (item.TemperatureC < 0)
                    {
                        producer.Publish("weather.hot.mid.cold", "weather.*.*.cold", null, message);
                    }
                    else if (item.TemperatureC > 0 && item.TemperatureC < 30)
                    {
                        producer.Publish("weather.hot.mid.cold", "weather.*.mid.*", null, message);
                    }
                    else
                    {
                        producer.Publish("weather.hot.mid.cold", "weather.hot.*.*", null, message);
                    }
                }
            };
            
            return weather;
        }
    }
}