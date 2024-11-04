using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Configuration
{
    public class PartnerServiceConfiguationDto
    {
        public virtual string ProviderCode { get; set; }
        public virtual string ProviderName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public CommonConst.PartnerServiceConfigurationStatus Status { get; set; }
        public string AccountCode { get; set; }
    }
}
