using System;
using RabbitMQ.Client;

namespace AVStack.MessageBus.Abstraction
{
    public interface IMessageProducer : IDisposable
    {
        void Publish(string exchange, string routingKey, IBasicProperties properties, string message);
        // Task PublishAsync(string exchange, string routingKey, IBasicProperties properties, string message);
    }
}