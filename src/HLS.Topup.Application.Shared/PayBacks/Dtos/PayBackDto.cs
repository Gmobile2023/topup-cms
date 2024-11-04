using System;
using Abp.Application.Services.Dto;
using HLS.Topup.Common;

namespace HLS.Topup.PayBacks.Dtos
{
    public class PayBackDto : EntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
        
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
        
        public int? Total { get; set; }
        
        public decimal? TotalAmount { get; set; }
        
        public CommonConst.PayBackStatus Status { get; set; }
        
        public DateTime? DateApproved { get; set; }
        
        public DateTime? DatePay { get; set; }
        
        public string UserApproved { get; set; }
        
        public DateTime? CreationTime { get; set; }
        
        public long? UserId { get; set; }
    }
}