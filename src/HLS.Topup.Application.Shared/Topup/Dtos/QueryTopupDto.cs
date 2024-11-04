using System;
using System.Collections.Generic;
using System.Text;

namespace HLS.Topup.Topup.Dtos
{
   
    public class QueryItemDto
    {
        public string ReceiverInfo { get; set; }
        public decimal Value { get; set; }
        public int Quantity { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string IsPayBill { get; set; }

    }

    public class SalePriceQueryDto
    {
        public string ReceiverInfo { get; set; }
        public decimal Value { get; set; }
        public decimal Discount { get; set; }
        public decimal Fee { get; set; }
        public decimal Price { get; set; }

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ServiceCode { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public string Provider { get; set; }
    }
}
