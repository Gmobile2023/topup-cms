using Abp.AutoMapper;
using HLS.Topup.MultiTenancy.Dto;

namespace HLS.Topup.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(RegisterTenantOutput))]
    public class TenantRegisterResultViewModel : RegisterTenantOutput
    {
        public string TenantLoginAddress { get; set; }
    }
}