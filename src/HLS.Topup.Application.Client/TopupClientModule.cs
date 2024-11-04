using Abp.Modules;
using Abp.Reflection.Extensions;

namespace HLS.Topup
{
    public class TopupClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupClientModule).GetAssembly());
        }
    }
}
