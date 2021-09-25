using System;
using AVStack.MessageBus.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Example.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add MessageBus 
            services.AddMessageBus(options =>
            {
                // MessageBus Configuration

                // options.UserName = Configuration.GetSection("RabbitMQ")["UserName"];
                // options.Password = Configuration.GetSection("RabbitMQ")["Password"];
                options.ClientProvidedName = "app:example";
                options.Uri = new Uri(Configuration.GetSection("RabbitMQ")["Uri"]);
            }, busFactory =>
            {
                busFactory.DeclareExchange("weather.hot.mid.cold", ExchangeType.Topic);
                busFactory.DeclareQueue("hot");
                busFactory.DeclareQueue("mid");
                busFactory.DeclareQueue("cold");
                busFactory.DeclareQueue("all");
                busFactory.BindQueue("hot", "weather.hot.mid.cold", "*.hot.*.*");
                busFactory.BindQueue("mid", "weather.hot.mid.cold", "*.*.mid.*");
                busFactory.BindQueue("cold", "weather.hot.mid.cold", "*.*.*.cold");
                busFactory.BindQueue("all", "weather.hot.mid.cold", "#");
            });

            services.AddHostedService<RabbitMqListener>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Example.WebApi",
                        Version = "v1",
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example.WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}