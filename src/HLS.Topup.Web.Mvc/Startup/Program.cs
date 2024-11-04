using System.IO;
using Microsoft.AspNetCore.Hosting;
using HLS.Topup.Web.Helpers;
using Microsoft.Extensions.Logging;
using System.Net;

namespace HLS.Topup.Web.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceStackHelper.SetLicense();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            CurrentDirectoryHelpers.SetCurrentDirectory();
           CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return new WebHostBuilder()
                .UseKestrel(opt =>
                {
                    opt.AddServerHeader = false;
                    opt.Limits.MaxRequestLineSize = 16 * 1024;

                    opt.Limits.MaxRequestBodySize = 100*1024 * 1024;

                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIIS()
                .UseIISIntegration()
                // .ConfigureLogging(logging =>
                // {
                //     //logging.ClearProviders();
                //     logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
                // })
                // .ConfigureLogging(p => p.AddConsole())
                .UseStartup<Startup>();
        }
    }
}
