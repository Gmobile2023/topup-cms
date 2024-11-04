using Abp.Application.Services.Dto;
using HLS.Topup.Common;

namespace HLS.Topup.Configuration.PartnerServiceConfigurationDtos
{
    public class PartnerServiceConfigurationDto : EntityDto
    {
        public string Name { get; set; }

        public CommonConst.PartnerServiceConfigurationStatus Status { get; set; }

        public int? ServiceId { get; set; }

        public int? ProviderId { get; set; }

        public int? CategoryId { get; set; }

        public long? UserId { get; set; }
        public string Description { get; set; }
    }
}