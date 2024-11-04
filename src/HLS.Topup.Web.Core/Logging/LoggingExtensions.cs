using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;

namespace HLS.Topup.Web.Logging
{
    public static class LoggingExtensions
    {
        public static void RegisterLogging(this IServiceCollection services, LoggingConfigDto serviceConfig)
        {
            if (serviceConfig == null)
            {
                throw new ArgumentNullException(nameof(serviceConfig));
            }

            Serilog.Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.WithProperty("Application", serviceConfig.Application)
                .Enrich.FromLogContext()
                .WriteTo.File(serviceConfig.LogFileUrl, outputTemplate: serviceConfig.OutputTemplate,
                    rollingInterval: RollingInterval.Day, retainedFileCountLimit: null)
                .WriteTo.Console(theme: AnsiConsoleTheme.Literate)
                .Filter.ByExcluding(e =>
                {
                    // TODO: should check for key existence in dictionary
                    var propertyValue = e.Properties[Constants.SourceContextPropertyName];
                    if (propertyValue != null)
                    {
                        var val = propertyValue.ToString().Replace("\"", "");
                        return (val.StartsWith("Microsoft.") && e.Level == LogEventLevel.Information);
                    }
                    return false;
                })
                .WriteTo.Elasticsearch(
                    new ElasticsearchSinkOptions(new Uri(serviceConfig.LogServer))
                    {
                        AutoRegisterTemplate = serviceConfig.AutoRegisterTemplate,
                        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                        IndexFormat = serviceConfig.IndexFormat,
                        ModifyConnectionSettings = x => x.BasicAuthentication(serviceConfig.UserName, serviceConfig.Password)
                    })
                .CreateLogger();
        }
    }
}
