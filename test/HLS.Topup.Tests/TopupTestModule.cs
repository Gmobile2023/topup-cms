using Abp.Modules;
using HLS.Topup.Test.Base;

namespace HLS.Topup.Tests
{
    [DependsOn(typeof(TopupTestBaseModule))]
    public class TopupTestModule : AbpModule
    {
       
    }
}
