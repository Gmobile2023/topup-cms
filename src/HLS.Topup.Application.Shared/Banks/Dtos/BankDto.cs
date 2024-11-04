using HLS.Topup.Common;
using System;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Banks.Dtos
{
    public class BankDto : EntityDto
    {
        public string BankName { get; set; }

        public string ShortName { get; set; }

        public string BranchName { get; set; }

        public string BankAccountName { get; set; }

        public string BankAccountCode { get; set; }

        public CommonConst.BankStatus Status { get; set; }

        public string Images { get; set; }
        
        public string SmsPhoneNumber { get; set; }
        
        public string SmsGatewayNumber { get; set; }
        
        // chưa dùng ở chỗ khác ( chỉ dùng cho phần tự động nạp tiền)
        // public string SmsSyntax { get; set; }
        // public string NoteSyntax { get; set; }
    }
}