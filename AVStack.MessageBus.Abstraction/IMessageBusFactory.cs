using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AVStack.MessageBus.Abstraction
{
    public interface IMessageBusFactory : IDisposable
    {
        Task Connect(CancellationToken cancellationToken = default);

        bool DeclareExchange(string exchange, string type, bool durable = true, bool autoDelete = false, IDictionary<string, object> properties = null);

        bool DeclareQueue(string queue, bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> properties = null);

        bool BindExchange(string exchangeTo, string exchangeFrom, string routingKey);

        bool BindQueue(string queue, string exchange, string routingKey);

        IMessageProducer CreateProducer();
        IMessageConsumer CreateConsumer();
    }
}