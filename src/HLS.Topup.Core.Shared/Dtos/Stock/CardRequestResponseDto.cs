using System;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Stock
{
    public class CardRequestResponseDto
    {
        public Guid Id { get; set; }
        public string Serial { get; set; }
        public string CardCode { get; set; }
        public int RequestValue { get; set; }
        public int RealValue { get; set; }
        public CommonConst.CardRequestStatus Status { get; set; }
        public DateTime DateUsed { get; set; }
        public string StockType { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime StartProcessTime { get; set; }
        public DateTime EndProcessTime { get; set; }
        public string ProviderCode { get; set; }
        public string SupplierCode { get; set; }
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public bool ProcessPartner { get; set; }//Giao dịch gọi sang kênh đối tác
        public string PartnerTransCode { get; set; }//Mã giao dịch khi gọi sang kênh đối tác
        public string CallBackUrl { get; set; }
        public int WaitingTimeInSeconds { get; set; }
        public string RefStatus { get; set; }
    }
}