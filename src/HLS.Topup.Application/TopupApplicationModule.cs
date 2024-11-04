using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using HLS.Topup.Authorization;

namespace HLS.Topup
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(TopupApplicationSharedModule),
        typeof(TopupCoreModule)
        )]
    public class TopupApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupApplicationModule).GetAssembly());
        }
    }
}