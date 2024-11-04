using Abp.Modules;
using Abp.Reflection.Extensions;

namespace HLS.Topup
{
    public class TopupCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupCoreSharedModule).GetAssembly());
        }
    }
}