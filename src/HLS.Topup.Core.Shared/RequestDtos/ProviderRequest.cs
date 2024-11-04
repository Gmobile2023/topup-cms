using System.Collections.Generic;
using HLS.Topup.Dtos.Provider;
using ServiceStack;

namespace HLS.Topup.RequestDtos
{
    [Route("/api/v1/topupgate/provider_info", "GET")]
    public class ProviderInfoGetRequest : IGet, IReturn<NewMessageReponseBase<string>>
    {
        public string ProviderCode { get; set; }
    }
    [Route("/api/v1/topupgate/provider_info", "PATCH")]
    public class ProviderInfoUpdateRequest : IPatch, IReturn<NewMessageReponseBase<string>>
    {
        public string ProviderCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiUrl { get; set; }
        public string ApiUser { get; set; }
        public string ApiPassword { get; set; }
        public string ExtraInfo { get; set; }
        public List<ProviderServiceDto> ProviderServices { get; set; }
        public int Timeout { get; set; }
        public int TimeoutProvider { get; set; }
        public string PrivateKeyFile { get; set; }
        public string PublicKeyFile { get; set; }
        public string PublicKey { get; set; }
        public int TotalTransError { get; set; }
        public int TimeClose { get; set; }
        public bool IsAutoCloseFail { get; set; }
        public string IgnoreCode { get; set; }
        public int TimeScan { get; set; }
        public int TotalTransScan { get; set; }
        public int TotalTransDubious { get; set; }
        public int TotalTransErrorScan { get; set; }
        public string ParentProvider { get; set; }
        public bool IsAlarm { get; set; }//Bật cảnh báo
        public string ErrorCodeNotAlarm { get; set; }//Bỏ qua các mã lỗi không cảnh báo
        public string MessageNotAlarm { get; set; }//Bỏ qua các message không cảnh báo
        public string AlarmChannel { get; set; }
        public string AlarmTeleChatId { get; set; }
        public int ProcessTimeAlarm { get; set; }//Cảnh báo thời gian xử lý giao dịch
    }
    [Route("/api/v1/topupgate/provider_info", "POST")]
    public class ProviderInfoCreateRequest : IPost, IReturn<NewMessageReponseBase<string>>
    {
        public string ProviderCode { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ApiUrl { get; set; }
        public string ApiUser { get; set; }
        public string ApiPassword { get; set; }
        public string ExtraInfo { get; set; }
        public List<ProviderServiceDto> ProviderServices { get; set; }
        public int Timeout { get; set; }
        public int TimeoutProvider { get; set; }
        public string PrivateKeyFile { get; set; }
        public string PublicKeyFile { get; set; }
        public string PublicKey { get; set; }
        public int TotalTransError { get; set; }
        public int TimeClose { get; set; }
        public bool IsAutoCloseFail { get; set; }
        public string IgnoreCode { get; set; }
        public int TimeScan { get; set; }
        public int TotalTransScan { get; set; }
        public int TotalTransDubious { get; set; }
        public int TotalTransErrorScan { get; set; }
        public string ParentProvider { get; set; }
        public bool IsAlarm { get; set; }//Bật cảnh báo
        public string ErrorCodeNotAlarm { get; set; }//Bỏ qua các mã lỗi không cảnh báo
        public string MessageNotAlarm { get; set; }//Bỏ qua các message không cảnh báo
        public string AlarmChannel { get; set; }
        public string AlarmTeleChatId { get; set; }
        public int ProcessTimeAlarm { get; set; }//Cảnh báo thời gian xử lý giao dịch
    }
}
