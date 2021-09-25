using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AVStack.MessageBus.Extensions;
using AVStack.MessageBus.RabbitMQ.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Example.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(app => app.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true))
                // .ConfigureServices((context, services) =>
                // {
                //     services.AddHostedService<RabbitMqListener>();
                // })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}