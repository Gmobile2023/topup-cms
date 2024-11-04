using HLS.Topup.Dtos.Services;
using ServiceStack;

namespace HLS.Topup.RequestDtos
{
    [Route("/api/v1/backend/service", "GET")]
    public class GetServiceRequest : IGet, IReturn<NewMessageReponseBase<ServiceConfigDto>>
    {
        public string ServiceCode { get; set; }
    }

    [Route("/api/v1/backend/service/create-update", "POST")]
    public class CreateOrUpdateServiceRequest : IPost, IReturn<NewMessageReponseBase<object>>
    {
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
