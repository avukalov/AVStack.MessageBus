using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AVStack.MessageBus.Abstraction;
using RabbitMQ.Client;

namespace AVStack.MessageBus.RabbitMQ
{
    public class MessageBusFactory : IMessageBusFactory
    {
        private IConnection _connection;

        private MessageBusFactory(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
        }

        // private event EventHandler ConnectionEstablished;

        private static IConnectionFactory Configure()
        {
            return new ConnectionFactory()
            {
                //AutomaticRecoveryEnabled = false,
                DispatchConsumersAsync = true,
                UseBackgroundThreadsForIO = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(60)
            };
        }

        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static IMessageBusFactory Configure(Action<IConnectionFactory> factoryOptionsAction = null)
        {
            var connectionFactory = Configure();
            factoryOptionsAction?.Invoke(connectionFactory);
            
            return new MessageBusFactory(connectionFactory);
        }
        
        // TODO: Implement connection with retry mechanism 
        public Task Connect(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public bool DeclareExchange(string exchange, string type, bool durable = true, bool autoDelete = false,
            IDictionary<string, object> properties = null)
        {
            // ReSharper disable once ConvertToUsingDeclaration
            using (var model = _connection.CreateModel())
            {
                try
                {
                    model.ExchangeDeclare(exchange, type, durable, autoDelete, properties);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }

        public bool DeclareQueue(string name, bool durable = true, bool exclusive = false, bool autoDelete = false,
            IDictionary<string, object> properties = null)
        {
            // ReSharper disable once ConvertToUsingDeclaration
            using (var model = _connection.CreateModel())
            {
                try
                {
                    model.QueueDeclare(name, durable, exclusive, autoDelete, properties);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }

        public bool BindQueue(string queueName, string exchangeName, string routingKey)
        {
            // ReSharper disable once ConvertToUsingDeclaration
            using (var model = _connection.CreateModel())
            {
                try
                {
                    model.QueueBind(queueName, exchangeName, routingKey);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }
        }

        public IMessageProducer CreateProducer()
        {
            return new MessageProducer(_connection.CreateModel());
        }

        public IMessageConsumer CreateConsumer()
        {
            return new MessageConsumer(_connection.CreateModel());
        }

        public void AddTopology(Action<IMessageBusFactory> topologyConfig)
        {
            topologyConfig.Invoke(this);
        }

        public void Dispose()
        {
            if (_connection == null) return;
            if (_connection.IsOpen) _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
    }
}