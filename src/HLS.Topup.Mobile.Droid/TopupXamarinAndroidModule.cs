using Abp.Modules;
using Abp.Reflection.Extensions;

namespace HLS.Topup
{
    [DependsOn(typeof(TopupXamarinSharedModule))]
    public class TopupXamarinAndroidModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupXamarinAndroidModule).GetAssembly());
        }
    }
}