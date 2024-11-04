using Abp.Application.Services.Dto;

namespace HLS.Topup.Configuration.PartnerServiceConfigurationDtos
{
    public class PartnerServiceConfigurationStatusResponseLookupTableDto
    {
        public int Id { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }
    }
}