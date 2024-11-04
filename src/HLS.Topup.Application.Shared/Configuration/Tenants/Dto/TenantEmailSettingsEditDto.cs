using Abp.Auditing;
using HLS.Topup.Configuration.Dto;

namespace HLS.Topup.Configuration.Tenants.Dto
{
    public class TenantEmailSettingsEditDto : EmailSettingsEditDto
    {
        public bool UseHostDefaultEmailSettings { get; set; }
    }
}