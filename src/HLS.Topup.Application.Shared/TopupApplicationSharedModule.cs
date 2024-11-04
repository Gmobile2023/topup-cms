using Abp.Modules;
using Abp.Reflection.Extensions;

namespace HLS.Topup
{
    [DependsOn(typeof(TopupCoreSharedModule))]
    public class TopupApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupApplicationSharedModule).GetAssembly());
        }
    }
}