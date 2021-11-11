using RabbitMQ.Client;

namespace AVStack.MessageBus.Extensions
{
    public static class Extensions
    {
        public static void SetDefaultValues(this IBasicProperties properties, string appId = null)
        {
            properties.AppId = appId ?? "";

            properties.ClearContentType();
            properties.ContentType = "application/json";
        }
    }
}