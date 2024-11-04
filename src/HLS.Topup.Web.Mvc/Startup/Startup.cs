using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using Abp.AspNetCore;
using Abp.AspNetCore.Mvc.Antiforgery;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.AspNetZeroCore.Web.Authentication.JwtBearer;
using Abp.Castle.Logging.Log4Net;
using Abp.Hangfire;
using Abp.PlugIns;
using Castle.Facilities.Logging;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Hangfire;
using Hangfire.PostgreSql;
using HealthChecks.UI.Client;
using HLS.Topup.Authorization;
using HLS.Topup.Configuration;
using HLS.Topup.Configure;
using HLS.Topup.EntityFrameworkCore;
using HLS.Topup.EventBus.Configure;
using HLS.Topup.Identity;
using HLS.Topup.Schemas;
using HLS.Topup.StockManagement;
using HLS.Topup.Web.Chat.SignalR;
using HLS.Topup.Web.Common;
using HLS.Topup.Web.Filters;
using HLS.Topup.Web.HealthCheck;
using HLS.Topup.Web.IdentityServer;
using HLS.Topup.Web.Logging;
using HLS.Topup.Web.Redis;
using HLS.Topup.Web.Resources;
using HLS.Topup.Web.Swagger;
using HLS.Topup.Web.TagHelpers;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Owl.reCAPTCHA;
using Serilog;
using Stripe;
using HealthChecksUISettings = HealthChecks.UI.Configuration.Settings;

namespace HLS.Topup.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            _hostingEnvironment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(config =>
            {
                // clear out default configuration
                //config.ClearProviders();
                //config.AddDebug();
                //config.AddNLog();
                config.AddSerilog();
                //config.AddEventSourceLogger();
                //config.AddConsole();
            });

            // MVC
            services.AddControllersWithViews(options =>
                {
                    options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
                })
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif
                .AddNewtonsoftJson();
            if (bool.Parse(_appConfiguration["KestrelServer:IsEnabled"]))
            {
                ConfigureKestrel(services);

                if (_appConfiguration.GetValue<bool>("Kestrel:Certificates:Default:ForwardHeader"))
                    services.Configure<ForwardedHeadersOptions>(options =>
                    {
                        options.ForwardedHeaders =
                            ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                        options.KnownProxies.Add(
                            IPAddress.Parse(_appConfiguration["Kestrel:Certificates:Default:ProxyIP"]));
                    });
            }

            IdentityRegistrar.Register(services);


            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.AddIdentityServer(services, _appConfiguration, options =>
                    options.UserInteraction =
                        new UserInteractionOptions
                        {
                            LoginUrl = "/Account/Login",
                            LogoutUrl = "/Account/LogOut",
                            ErrorUrl = "/Error"
                        }, typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
            }

            AuthConfigurer.Configure(services, _appConfiguration);
            if (bool.Parse(_appConfiguration["EventBus:IsEnabled"]))
            {
                services.AddConfigureForEventBus(_appConfiguration);
            }

            if (WebConsts.SwaggerUiEnabled)
                //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo {Title = "Topup API", Version = "v1"});
                    options.DocInclusionPredicate((docName, description) => true);
                    options.ParameterFilter<SwaggerEnumParameterFilter>();
                    options.SchemaFilter<SwaggerEnumSchemaFilter>();
                    options.OperationFilter<SwaggerOperationIdFilter>();
                    options.OperationFilter<SwaggerOperationFilter>();
                    options.CustomDefaultSchemaIdSelector();
                }).AddSwaggerGenNewtonsoftSupport();

            //Recaptcha
            services.AddreCAPTCHAV3(x =>
            {
                x.SiteKey = _appConfiguration["Recaptcha:SiteKey"];
                x.SiteSecret = _appConfiguration["Recaptcha:SecretKey"];
            });

            if (bool.Parse(_appConfiguration["App:AllowHangFireServer"]))
            {
                //Hangfire (Enable to use Hangfire instead of default job manager)
                services.AddHangfire(config =>
                {
                    config.UsePostgreSqlStorage(_appConfiguration.GetConnectionString("HangFire"));
                });
                //services.AddHangfireServer(options => { options.Queues = new[] {"default"}; });
            }

            services.AddScoped<IWebResourceManager, WebResourceManager>();

            services.AddSignalR();

            if (WebConsts.GraphQL.Enabled) services.AddAndConfigureGraphQL();

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                services.AddAbpZeroHealthCheck();

                var healthCheckUISection = _appConfiguration.GetSection("HealthChecks")?.GetSection("HealthChecksUI");

                if (bool.Parse(healthCheckUISection["HealthChecksUIEnabled"]))
                {
                    services.Configure<HealthChecksUISettings>(settings =>
                    {
                        healthCheckUISection.Bind(settings, c => c.BindNonPublicProperties = true);
                    });
                    services.AddHealthChecksUI()
                        .AddInMemoryStorage();
                }
            }

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new RazorViewLocationExpander());
            });
            services.AddScoped<IViewRender, ViewRender>();
            services.AddScoped<CheckUserAccessAreaFilter>();
            services.RegisterLogging(_appConfiguration.GetLoggingConfig());
            services.RegisterRedisSentinel(_appConfiguration);
            //services.AddScoped<ForgeryExceptionFilter>();
            //services.AddTransient<CheckUserRouterMiddleware>();//Chuyển qua dùng action filter
            //Configure Abp and Dependency Injection

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            var sv = services.AddAbp<TopupWebMvcModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                        ? "log4net.config"
                        : "log4net.Production.config")
                );

                options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"),
                    SearchOption.AllDirectories);
            });

            return sv;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (bool.Parse(_appConfiguration["KestrelServer:IsEnabled"]))
                if (_appConfiguration.GetValue<bool>("Kestrel:Certificates:Default:ForwardHeader"))
                {
                    app.UseForwardedHeaders();
                    app.Use((context, next) =>
                    {
                        context.Request.Scheme = "https";
                        return next();
                    });
                }

            //Initializes ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");
            }

            app.UseHttpsRedirection();
            app.UseCertificateForwarding();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();

            if (bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"])) app.UseJwtTokenMiddleware();

            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware("IdentityBearer");
                app.UseIdentityServer();
            }

            app.UseAuthorization();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<DatabaseCheckHelper>()
                    .Exist(_appConfiguration["ConnectionStrings:Default"])) app.UseAbpRequestLocalization();
            }

            if (bool.Parse(_appConfiguration["App:AllowHangFireServer"]))
            {
                //Hangfire dashboard & server (Enable to use Hangfire instead of default job manager)
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[]
                        {new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard)}
                });
                //Hangfire dashboard &server(Enable to use Hangfire instead of default job manager)
                app.UseHangfireDashboard(WebConsts.HangfireDashboardEndPoint, new DashboardOptions
                {
                    Authorization = new[]
                        {new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard)}
                });
                app.UseHangfireServer();
                // var time = int.Parse(_appConfiguration["HangfireConfig:DeleteBinaryObject:TimeRun"]);
                // if (bool.Parse(_appConfiguration["HangfireConfig:DeleteBinaryObject:IsRun"]))
                // {
                //     RecurringJob.AddOrUpdate<ExpiredStorageDeleterWorker>(x => x.DeleteBinaryObject(),
                //         $"0 {time} * * *");
                // }

                if (bool.Parse(_appConfiguration["HangfireConfig:MinStockAirtime:IsRun"]))
                {
                    RecurringJob.AddOrUpdate<IStockAirtimeManager>(x => x.AutoCheckBalanceProvider(),
                        $"*/{int.Parse(_appConfiguration["HangfireConfig:MinStockAirtime:TimeRun"])} * * * *");
                }
            }

            if (bool.Parse(_appConfiguration["Payment:Stripe:IsActive"]))
                StripeConfiguration.ApiKey = _appConfiguration["Payment:Stripe:SecretKey"];

            if (WebConsts.GraphQL.Enabled)
            {
                app.UseGraphQL<MainSchema>();
                if (WebConsts.GraphQL.PlaygroundEnabled)
                    app.UseGraphQLPlayground(
                        new GraphQLPlaygroundOptions()); //to explorer API navigate https://*DOMAIN*/ui/playground
            }

            //app.UseCheckUserRouterMiddleware();Chuyển qua dùng action filter
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapHub<ChatHub>("/signalr-chat");

                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksUI:HealthChecksUIEnabled"]))
                    app.UseHealthChecksUI();

            if (WebConsts.SwaggerUiEnabled)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();
                //Enable middleware to serve swagger - ui assets(HTML, JS, CSS etc.)
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "Topup API V1");
                    options.IndexStream = () => Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("HLS.Topup.Web.wwwroot.swagger.ui.index.html");
                    options.InjectBaseUrl(_appConfiguration["App:WebSiteRootAddress"]);
                }); //URL: /swagger
            }

            //loggerFactory.AddSerilog();
            //loggerFactory.AddNLog();
            //loggerFactory.AddProvider(new NLog.Extensions.Logging.NLogLoggerProvider());
        }

        private void ConfigureKestrel(IServiceCollection services)
        {
            // services.Configure<KestrelServerOptions>(_appConfiguration.GetSection("Kestrel"));
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Configure(_appConfiguration.GetSection("Kestrel"))
                    .Endpoint("Https",
                        listenOptions => { listenOptions.HttpsOptions.SslProtocols = SslProtocols.Tls12; });
                // options.Listen(
                //     new IPEndPoint(IPAddress.Any,
                //         //                       IPAddress.Parse(_appConfiguration.GetValue<string>("Kestrel:Certificates:Default:IP")),
                //         _appConfiguration.GetValue<int>("Kestrel:Certificates:Default:Port")),
                //     listenOptions =>
                //     {
                //         var certPassword = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Password");
                //         var certPath = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Path");
                //         var cert = new X509Certificate2(certPath,
                //             certPassword);
                //         listenOptions.UseHttps(new HttpsConnectionAdapterOptions
                //         {
                //             ServerCertificate = cert
                //         });
                //     });
                // options.Listen(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 443),
                //     listenOptions =>
                //     {
                //         var certPassword = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Password");
                //         var certPath = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Path");
                //         var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, certPassword);
                //         listenOptions.UseHttps(new HttpsConnectionAdapterOptions()
                //         {
                //             ServerCertificate = cert
                //         });
                //     });
            });
        }

        // private void ConfigureKestrel(IServiceCollection services)
        // {
        //     services.Configure<Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions>(options =>
        //     {
        //         options.Listen(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 443),
        //             listenOptions =>
        //             {
        //                 var certPassword = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Password");
        //                 var certPath = _appConfiguration.GetValue<string>("Kestrel:Certificates:Default:Path");
        //                 var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, certPassword);
        //                 listenOptions.UseHttps(new HttpsConnectionAdapterOptions()
        //                 {
        //                     ServerCertificate = cert
        //                 });
        //             });
        //     });
        // }
    }
}