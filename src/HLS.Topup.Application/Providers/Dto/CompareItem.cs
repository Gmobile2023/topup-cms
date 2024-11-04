using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Providers.Dto
{
    public class CompareItem
    {
        public string ReceivedAccount { get; set; }

        public string TransCode { get; set; }

        public string CreatedDate { get; set; }

        public string Status { get; set; }

        public string Amount { get; set; }

        public string CompareDate { get; set; }

        public string AccountCode { get; set; }

        public string ProviderCode { get; set; }
    }

    public class VTDDItem
    {
        public int Index { get; set; }

        public string Mobile { get; set; }

        public string Gateway { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public string Ukey { get; set; }

        public string TransCodePay { get; set; }

        public string CreateDate { get; set; }
    }

    public class IOMediaItem
    {
        public int Index { get; set; }

        public string Telco { get; set; }

        public string Type { get; set; }

        public decimal ProductValue { get; set; }

        public string Mobile { get; set; }

        public decimal Fix { get; set; }

        public decimal Discount { get; set; }

        public decimal Amount { get; set; }

        public string CreateDate { get; set; }

        public string TransCodePay { get; set; }
        
    }
}
