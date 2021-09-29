using System;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace AVStack.MessageBus.Extensions
{
    public static class Serializer
    {
        // TODO: Implement IBasicProperties class
        public static T Deserialize<T>(ReadOnlyMemory<byte> body, string contentType = null) where T : class
        {
            var bodyString = Encoding.UTF8.GetString(body.ToArray());

            return contentType switch
            {
                "application/json" => JsonSerializer.Deserialize<T>(bodyString),
                _ => JsonSerializer.Deserialize<T>(bodyString)
            };
        }
    }
}