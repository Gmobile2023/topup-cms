using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using HLS.Topup.Configure;
using HLS.Topup.Startup;
using HLS.Topup.Test.Base;

namespace HLS.Topup.GraphQL.Tests
{
    [DependsOn(
        typeof(TopupGraphQLModule),
        typeof(TopupTestBaseModule))]
    public class TopupGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(TopupGraphQLTestModule).GetAssembly());
        }
    }
}