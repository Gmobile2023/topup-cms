using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace HLS.Topup
{
    [DependsOn(typeof(TopupClientModule), typeof(AbpAutoMapperModule))]
    public class TopupXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupXamarinSharedModule).GetAssembly());
        }
    }
}