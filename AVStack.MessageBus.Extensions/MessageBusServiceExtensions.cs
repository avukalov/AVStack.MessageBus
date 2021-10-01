using System;
using AVStack.MessageBus.Abstraction;
using AVStack.MessageBus.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace AVStack.MessageBus.Extensions
{
    public static class MessageBusServiceExtensions
    {
        public static void AddMessageBus(
            this IServiceCollection services,
            Action<IConnectionFactory> factoryOptionsAction,
            Action<IMessageBusFactory> busFactoryAction = null)
        {
            var busFactory = MessageBusFactory.Configure(factoryOptionsAction);
            busFactoryAction?.Invoke(busFactory);
            services.TryAddSingleton(busFactory);
        }
    }
}