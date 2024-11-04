using Abp.Application.Services.Dto;

namespace HLS.Topup.Configuration.Dtos
{
    public class ServiceConfigurationStatusResponseLookupTableDto
    {
        public int Id { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseMessage { get; set; }
    }
}