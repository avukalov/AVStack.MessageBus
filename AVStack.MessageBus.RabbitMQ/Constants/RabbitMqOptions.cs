namespace AVStack.MessageBus.RabbitMQ.Constants
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = nameof(RabbitMqOptions.RabbitMq);
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Uri { get; set; }
    }
}