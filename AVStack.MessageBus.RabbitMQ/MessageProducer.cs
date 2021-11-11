using AVStack.MessageBus.Abstraction;
using RabbitMQ.Client;

namespace AVStack.MessageBus.RabbitMQ
{
    public class MessageProducer : MessageBusBase, IMessageProducer
    {
        private readonly IModel _model;

        public MessageProducer(IModel model) : base(model)
        {
            _model = model;
        }

    }
}