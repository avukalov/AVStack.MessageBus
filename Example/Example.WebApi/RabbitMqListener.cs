using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AVStack.MessageBus.Abstraction;
using AVStack.MessageBus.Extensions;
using Example.WebApi.Models;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Events;

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
            
            consumerCold.ConsumeAsync("cold", async (model, ea) =>
            {
                var weatherItem = Serializer.Deserialize<WeatherForecast>(ea.Body, ea.BasicProperties.ContentType);

                consumerCold.BasicAck(ea.DeliveryTag);
                Console.WriteLine($"COLD ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");

                await Task.FromResult(true);
            });

            consumerMid.ConsumeAsync("mid", async (model, ea) =>
            {
                var weatherItem = Serializer.Deserialize<WeatherForecast>(ea.Body, ea.BasicProperties.ContentType);
                consumerMid.BasicAck(ea.DeliveryTag);
                
                Console.WriteLine($"MID ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");
                
                await Task.FromResult(true);
            });
            
            consumerHot.ConsumeAsync("hot", async (model, ea) =>
            {
                var weatherItem = Serializer.Deserialize<WeatherForecast>(ea.Body, ea.BasicProperties.ContentType);
                consumerHot.BasicAck(ea.DeliveryTag);
                
                Console.WriteLine($"HOT ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");
                
                await Task.FromResult(true);
            });
            
            consumerAll.ConsumeAsync("all", async (model, ea) =>
            {
                var weatherItem = Serializer.Deserialize<WeatherForecast>(ea.Body, ea.BasicProperties.ContentType);
                consumerAll.BasicAck(ea.DeliveryTag);
                
                Console.WriteLine($"ALL ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");
                
                await Task.FromResult(true);
            });


            return Task.CompletedTask;
        }
    }
}