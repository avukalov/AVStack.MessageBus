using AVStack.MessageBus.Abstraction;
using RabbitMQ.Client.Events;

namespace Example.WebApi.Services
{
    public class WeatherService
    {
        // private readonly IMessageBusFactory _busFactory;
        //
        // public WeatherService(IMessageBusFactory busFactory)
        // {
        //     _busFactory = busFactory;
        // }
        //
        // public virtual void OnMessageReceived(object model, BasicDeliverEventArgs eventArgs)
        // {
        //     consumerAll.Consume("all", async (model, ea) =>
        //     {
        //         var body = ea.Body.ToArray();
        //         var message = Encoding.UTF8.GetString(body);
        //         var weatherItem = JsonSerializer.Deserialize<WeatherForecast>(message);
        //         Console.WriteLine($"ALL ({ea.RoutingKey}) -> {weatherItem?.TemperatureC}°C");
        //         consumerAll.BasicAck(ea.DeliveryTag);
        //     });
        // }
    }
}