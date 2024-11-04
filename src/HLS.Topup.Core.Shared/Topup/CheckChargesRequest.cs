﻿using ServiceStack;
using System;
using HLS.Topup.Common;

namespace HLS.Topup.Topup
{
    [Route("/api/v1/checkcharges/history", "GET")]
    public class CheckChargesRequest: PaggingBaseDto
    {
        public string CodeRequest { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    [Route("/api/v1/checkcharges/history/{CodeRequest}", "GET")]
    public class CheckChargesDetailRequest : PaggingBaseDto
    {
        public string CodeRequest { get; set; }
        public string PhoneNumber { get; set; }
    }
}
