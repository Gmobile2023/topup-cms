using System.Collections.Generic;
using HLS.Topup.Dtos.Partner;
using ServiceStack;

namespace HLS.Topup.RequestDtos
{
    [Route("/api/v1/backend/partner/create", "POST")]
    public class CreatePartnerRequest : IPost, IReturn<NewMessageReponseBase<object>>
    {
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PublicKeyFile { get; set; }
        public string PrivateKeyFile { get; set; }
        public string SecretKey { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSig { get; set; }
        public bool IsActive { get; set; }
        public List<string> ServiceConfigs { get; set; }
        public List<string> CategoryConfigs { get; set; }
        public int LastTransTimeConfig { get; set; }
        public string Ips { get; set; }
        public bool IsCheckAllowTopupReceiverType { get; set; }
        public string DefaultReceiverType { get; set; }
    }

    [Route("/api/v1/backend/partner/update", "PUT")]
    public class UpdatePartnerRequest : IPut, IReturn<NewMessageReponseBase<object>>
    {
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PublicKeyFile { get; set; }
        public string PrivateKeyFile { get; set; }
        public string SecretKey { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSig { get; set; }
        public bool IsActive { get; set; }
        public bool IsCheckReceiverType { get; set; }
        public bool IsNoneDiscount { get; set; }
        public bool IsCheckPhone { get; set; }
        public int LastTransTimeConfig { get; set; }
        public List<string> ServiceConfigs { get; set; }
        public List<string> CategoryConfigs { get; set; }
        public string Ips { get; set; }
        public bool IsCheckAllowTopupReceiverType { get; set; }
        public string DefaultReceiverType { get; set; }
    }

    [Route("/api/v1/backend/partner", "GET")]
    public class GetPartnerRequest : IGet, IReturn<NewMessageReponseBase<PartnerConfigTransDto>>
    {
        public string PartnerCode { get; set; }
    }
    [Route("/api/v1/backend/partner/create-update", "POST")]
    public class CreateOrUpdatePartnerRequest : IPost, IReturn<NewMessageReponseBase<object>>
    {
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string PublicKeyFile { get; set; }
        public string PrivateKeyFile { get; set; }
        public string SecretKey { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSig { get; set; }
        public bool IsActive { get; set; }
        public bool IsCheckReceiverType { get; set; }
        public bool IsNoneDiscount { get; set; }
        public bool IsCheckPhone { get; set; }
        public int LastTransTimeConfig { get; set; }
        public List<string> ServiceConfigs { get; set; }
        public List<string> CategoryConfigs { get; set; }
        public List<string> ProductConfigsNotAllow { get; set; }
        public string Ips { get; set; }
        public int MaxTotalTrans { get; set; }
        public bool IsCheckAllowTopupReceiverType { get; set; }
        public string DefaultReceiverType { get; set; }
    }
}
