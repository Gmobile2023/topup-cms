using Abp.AutoMapper;
using HLS.Topup.MultiTenancy.Dto;

namespace HLS.Topup.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
