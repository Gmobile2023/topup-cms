using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace HLS.Topup.Startup
{
    [DependsOn(typeof(TopupCoreModule))]
    public class TopupGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}