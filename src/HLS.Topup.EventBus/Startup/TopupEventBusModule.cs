using Abp.Modules;
using Abp.Reflection.Extensions;

namespace HLS.Topup.EventBus.Startup
{
    [DependsOn(typeof(TopupCoreModule))]
    public class TopupEventBusModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupEventBusModule).GetAssembly());
        }
    }
}
