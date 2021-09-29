using System;
using RabbitMQ.Client.Events;

namespace AVStack.MessageBus.Abstraction
{
    public interface IMessageConsumer : IDisposable
    {
        string Consume(string queue, bool autoAck = false);
        string Consume(string queue, EventHandler<BasicDeliverEventArgs> handler, bool autoAck = false);
        string ConsumeAsync(string queue, AsyncEventHandler<BasicDeliverEventArgs> handler, bool autoAck = false);
        void BasicAck(ulong deliveryTag, bool multiple = false);
    }
}