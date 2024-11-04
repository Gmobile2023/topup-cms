using System;
using Microsoft.Extensions.Configuration;

namespace HLS.Topup.Web.Logging
{
    public static class LoggingConfigExtentions
    {
        public static LoggingConfigDto GetLoggingConfig(this IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var serviceConfig = new LoggingConfigDto
            {
                LogServer = configuration["LoggingConfig:LogServer"],
                LogFileUrl = configuration["LoggingConfig:LogFileUrl"],
                OutputTemplate = configuration["LoggingConfig:OutputTemplate"],
                RetainedFileCountLimit = configuration["LoggingConfig:RetainedFileCountLimit"],
                IndexFormat = configuration["LoggingConfig:IndexFormat"],
                Application = configuration["LoggingConfig:Application"],
                UserName = configuration["LoggingConfig:UserName"],
                Password = configuration["LoggingConfig:Password"],
                AutoRegisterTemplate = bool.Parse(configuration["LoggingConfig:AutoRegisterTemplate"]),
            };
            return serviceConfig;
        }
    }
}
