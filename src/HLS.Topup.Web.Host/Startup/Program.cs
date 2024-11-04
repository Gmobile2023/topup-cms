using System.IO;
using Microsoft.AspNetCore.Hosting;
using HLS.Topup.Web.Helpers;
using Microsoft.Extensions.Configuration;
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
            // CreateWebHostBuilder(args).Build().Run();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            // return new WebHostBuilder()
            //     .UseKestrel(opt =>
            //     {
            //         opt.AddServerHeader = false;
            //         opt.Limits.MaxRequestLineSize = 16 * 1024;
            //     })
            //     .UseContentRoot(Directory.GetCurrentDirectory())
            //     .UseIIS()
            //     .UseIISIntegration()
            //     .UseStartup<Startup>();


            var config = new ConfigurationBuilder()
                //.AddCommandLine(args)
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json", optional: true)
                .Build();

            var host = new WebHostBuilder()
                .UseKestrel(opt =>
                {
                    opt.AddServerHeader = false;
                    opt.Limits.MaxRequestLineSize = 16 * 1024;
                })
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();
            return host;
        }
    }
}
