using System.Linq;
using HLS.Topup.EventBus.Consumers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HLS.Topup.EventBus.Configure
{
    public static class ServiceCollectionExtensions
    {
        public static void AddConfigureForEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            configuration = configuration.GetSection("EventBus");
            var clusters = configuration["RabbitMq:Clusters"].Split(";").ToList();
            if (bool.Parse(configuration["IsConsumer"]))
            {
                services.AddMassTransit(x =>
                {
                    x.AddConsumersFromNamespaceContaining<ProviderActionConsumer>();
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.AutoStart = true;
                        cfg.Host(configuration["RabbitMq:Host"], configuration["RabbitMq:VirtualHost"], h =>
                        {
                            h.Username(configuration["RabbitMq:Username"]);
                            h.Password(configuration["RabbitMq:Password"]);
                            h.UseCluster(p =>
                            {
                                foreach (var server in clusters)
                                    p.Node(server);
                            });
                        });
                        cfg.ApplyCustomBusConfiguration();
                        cfg.ConfigureEndpoints(context);
                    });
                });
            }
            else
            {
                services.AddMassTransit(x =>
                {
                    x.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.AutoStart = true;
                        cfg.Host(configuration["RabbitMq:Host"], configuration["RabbitMq:VirtualHost"], h =>
                        {
                            h.Username(configuration["RabbitMq:Username"]);
                            h.Password(configuration["RabbitMq:Password"]);
                            h.UseCluster(p =>
                            {
                                foreach (var server in clusters)
                                    p.Node(server);
                            });
                        });
                        cfg.ApplyCustomBusConfiguration();
                        cfg.ConfigureEndpoints(context);
                    });
                });
            }
        }
    }
}