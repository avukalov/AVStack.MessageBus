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

        /// <summary>
        /// Publish a message on RabbitMQ exchange with the specified routingKey and properties.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="properties"></param>
        /// <param name="message"></param>
        public virtual void Publish(string exchange, string routingKey, IBasicProperties properties, string message)
        {
            _model.BasicPublish(exchange, routingKey, false, properties, Encoding.UTF8.GetBytes(message));
        }

        /// <summary>
        /// Fetching individual messages (Pooling or "pull API").
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="autoAck"></param>
        /// <returns>Message body otherwise empty string</returns>
        public virtual string Consume(string queue, bool autoAck = false)
        {
            var result = _model.BasicGet(queue, autoAck);
            if (result == null)
            {
                return string.Empty;
            }

            var message = Encoding.UTF8.GetString(result.Body.ToArray());

            _model.BasicAck(result.DeliveryTag, false);
            return message;
        }

        /// <summary>
        /// Consume message from the specified queue
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handler"></param>
        /// <param name="autoAck"></param>
        /// <returns>Consumer tag</returns>
        public virtual string Consume(string queue, EventHandler<BasicDeliverEventArgs> handler, bool autoAck = false)
        {
            var consumer = new EventingBasicConsumer(_model);
            consumer.Received += handler;
            return _model.BasicConsume(queue, autoAck, consumer);
        }

        /// <summary>
        /// Consume message from the specified queue asynchronously
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="handler"></param>
        /// <param name="autoAck"></param>
        /// <returns>Consumer tag</returns>
        public virtual string ConsumeAsync(string queue, AsyncEventHandler<BasicDeliverEventArgs> handler, bool autoAck = false)
        {
            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += handler;
            return _model.BasicConsume(queue, autoAck, consumer);
        }

        /// <summary>
        /// Acknowledge that the message has been successfully received
        /// </summary>
        /// <param name="deliveryTag"></param>
        /// <param name="multiple"></param>
        public virtual void BasicAck(ulong deliveryTag, bool multiple = false)
        {
            _model.BasicAck(deliveryTag, multiple);
        }

        /// <summary>
        /// Creates IBasicProperties
        /// </summary>
        /// <returns><see cref="IBasicProperties"/></returns>
        public IBasicProperties CreateBasicProperties()
        {
            return _model.CreateBasicProperties();
        }

        public void Dispose()
        {
            if (_model.IsOpen)
            {
                _model.Close();
            }
            _model?.Dispose();
        }
    }
}