using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AVStack.MessageBus.Abstraction;
using Example.WebApi.Models;
using Microsoft.Extensions.Hosting;

namespace Example.WebApi
{
    public class RabbitMqListener : BackgroundService
    {
        private readonly IMessageBusFactory _busFactory;

        public RabbitMqListener(IMessageBusFactory busFactory)
        {
            _busFactory = busFactory;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var consumerCold = _busFactory.CreateConsumer();
            var consumerMid = _busFactory.CreateConsumer();
            var consumerHot = _busFactory.CreateConsumer();
            var consumerAll = _busFactory.CreateConsumer();
            
            consumerCold.BasicConsumeAsync("cold", async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var weatherItem = JsonSerializer.Deserialize<WeatherForecast>(message);
                consumerCold.BasicAck(ea.DeliveryTag);
                
                Console.WriteLine($"COLD ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");
                
                await Task.FromResult(true);
            });
            
            consumerMid.BasicConsumeAsync("mid", async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var weatherItem = JsonSerializer.Deserialize<WeatherForecast>(message);
                consumerMid.BasicAck(ea.DeliveryTag);
                
                Console.WriteLine($"MID ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");
                
                await Task.FromResult(true);
            });
            
            consumerHot.BasicConsumeAsync("hot", async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var weatherItem = JsonSerializer.Deserialize<WeatherForecast>(message);
                consumerHot.BasicAck(ea.DeliveryTag);
                
                Console.WriteLine($"HOT ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");
                
                await Task.FromResult(true);
            });
            
            consumerAll.BasicConsumeAsync("all", async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                var weatherItem = JsonSerializer.Deserialize<WeatherForecast>(message);
                consumerAll.BasicAck(ea.DeliveryTag);
                
                Console.WriteLine($"ALL ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");
                
                await Task.FromResult(true);
            });


            return Task.CompletedTask;
        }
    }
}