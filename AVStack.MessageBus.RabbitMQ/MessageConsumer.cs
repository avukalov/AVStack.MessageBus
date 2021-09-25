using System.Threading.Tasks;
using AVStack.MessageBus.Abstraction;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AVStack.MessageBus.RabbitMQ
{
    public class MessageConsumer : MessageBusBase, IMessageConsumer
    {
        private readonly IModel _model;

        public MessageConsumer(IModel model) : base(model)
        {
            _model = model;
        }
    }
}