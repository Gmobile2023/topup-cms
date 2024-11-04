using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;

namespace HLS.Topup.Web.Redis
{
    public static class RedisSentinelExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void RegisterRedisSentinel(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceConfig = new RedisConfig();
            configuration.GetSection("Abp:RedisCache").Bind(serviceConfig);

            if (serviceConfig == null)
            {
                throw new ArgumentNullException(nameof(serviceConfig));
            }

            services.AddSingleton<IRedisClientsManagerAsync>(c =>
                new RedisManagerPool(serviceConfig.RedisServer.Split(',', ';', '|')));
        }
    }
}