﻿using System;
using System.Collections.Generic;
using System.Text;
using HLS.Topup.Common;

namespace HLS.Topup.Topup.ResponseDto
{
    public class CheckChargesDetaiResponselDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public decimal DebtCharges { get; set; }
        public decimal RoundingCharges { get; set; }
        public decimal OddCharges { get; set; }
        public CommonConst.TransStatus Status { get; set; }
    }
    public class CheckChargesHistoryResponseDto
    {
        public Guid Id { get; set; }
        public string CodeRequest { get; set; }
        public int Amount { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal FeeRequest { get; set; }
    }
}
