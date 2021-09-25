using System;
using RabbitMQ.Client.Events;

namespace AVStack.MessageBus.Abstraction
{
    public interface IMessageConsumer : IDisposable
    {
        string BasicConsume(string queue, bool autoAck = false);

        void BasicConsumeAsync(string queue, AsyncEventHandler<BasicDeliverEventArgs> onMessageReceived, bool autoAck = false);
        void BasicAck(ulong deliveryTag, bool multiple = false);
    }
}