using Abp.AspNetZeroCore;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using HLS.Topup.Auditing;
using HLS.Topup.Configuration;
using HLS.Topup.EntityFrameworkCore;
using HLS.Topup.MultiTenancy;
using HLS.Topup.Web.Areas.App.Startup;

namespace HLS.Topup.Web.Startup
{
    [DependsOn(
        typeof(TopupWebCoreModule)
    )]
    public class TopupWebMvcModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public TopupWebMvcModule(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:WebSiteRootAddress"] ?? "https://localhost:44302/";
            Configuration.Modules.AspNetZero().LicenseCode = _appConfiguration["AbpZeroLicenseCode"];
            Configuration.Navigation.Providers.Add<AppNavigationProvider>();
            Configuration.Navigation.Providers.Add<LeftMenuNavigationProvider>();
            Configuration.Navigation.Providers.Add<MenuNavigationProvider>();
            
            IocManager.Register<DashboardViewConfiguration>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupWebMvcModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                return;
            }

            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
            workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
            workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());

            if (Configuration.Auditing.IsEnabled && ExpiredAuditLogDeleterWorker.IsEnabled)
            {
                workManager.Add(IocManager.Resolve<ExpiredAuditLogDeleterWorker>());
            }
        }
    }
}