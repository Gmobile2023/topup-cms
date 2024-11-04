using System;
using HLS.Topup.Common;
using ServiceStack;

namespace HLS.Topup.RequestDtos
{
    [Route("/api/v1/backend/limitration/set-limit-trans-amount", "POST")]
    public class CreateOrUpdateLimitAccountTransRequest
    {
        public string AccountCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
        public decimal LimitPerDay { get; set; }
        public decimal LimitPerTrans { get; set; }
    }
    [Route("/api/v1/backend/limitration/get-available-limit", "GET")]
    public class GetAvailableLimitAccount
    {
        public string AccountCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
    }
    [Route("/api/v1/backend/limitration/product/totalday", "GET")]
    public class GetTotalPerDayProductRequest
    {
        public string AccountCode { get; set; }
        public string ServiceCode { get; set; }
        public string CategoryCode { get; set; }
        public string ProductCode { get; set; }
    }
}
