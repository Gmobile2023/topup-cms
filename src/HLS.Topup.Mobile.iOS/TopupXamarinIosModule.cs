using Abp.Modules;
using Abp.Reflection.Extensions;

namespace HLS.Topup
{
    [DependsOn(typeof(TopupXamarinSharedModule))]
    public class TopupXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupXamarinIosModule).GetAssembly());
        }
    }
}