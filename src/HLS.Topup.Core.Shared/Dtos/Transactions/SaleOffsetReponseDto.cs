using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Dtos.Transactions
{
    public class SaleOffsetReponseDto
    {
        public string ReceiverInfo { get; set; }
        
        public decimal Amount { get; set; }

        public HLS.Topup.Common.CommonConst.SaleRequestStatus Status { get; set; }

        public string StatusName { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime OriginCreatedTime { get; set; }
        public string OriginPartnerCode { get; set; }
        public string OriginTransRef { get; set; }
        public string OriginTransCode { get; set; }

        public string OriginProviderCode { get; set; }

        public string ProductCode { get; set; }
        public string PartnerCode { get; set; }
        public string TransRef { get; set; }
        public string TransCode { get; set; }
        public string Vendor { get; set; }
        public string ServiceCode { get; set; }

        public string ServiceName { get; set; }

        public string ProviderCode { get; set; }
    }
}
