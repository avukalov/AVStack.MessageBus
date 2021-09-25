using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AVStack.MessageBus.RabbitMQ
{
    public abstract class MessageBusBase : AsyncEventingBasicConsumer, IDisposable
    {
        private readonly IModel _model;

        protected MessageBusBase(IModel model) : base(model)
        {
            _model = model;
        }

        public virtual void Publish(string exchange, string routingKey, IBasicProperties properties, string message)
        {
            _model.BasicPublish(exchange, routingKey, false, properties, Encoding.UTF8.GetBytes(message));
        }

        public virtual string BasicConsume(string queue, bool autoAck = false)
        {
            var result = _model.BasicGet(queue, autoAck);
            if (result == null) return "";
            var message = Encoding.UTF8.GetString(result.Body.ToArray());
            _model.BasicAck(result.DeliveryTag, false);
            return message;
        }
        
        public virtual void BasicConsume(string queue, EventHandler<BasicDeliverEventArgs> onMessageReceived, bool autoAck = false)
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += onMessageReceived;
            _model.BasicConsume(queue, autoAck, consumer);
        }
        
        public void BasicConsumeAsync(string queue, AsyncEventHandler<BasicDeliverEventArgs> onMessageReceived, bool autoAck = false)
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += onMessageReceived;
            _model.BasicConsume(queue, autoAck, consumer);
        }

        public void BasicAck(ulong deliveryTag, bool multiple = false)
        {
            _model.BasicAck(deliveryTag, multiple);
        }

        public void Dispose()
        {
            if(_model.IsOpen) _model.Close();
            _model?.Dispose();
        }
    }
}