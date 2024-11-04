using System;
using System.Runtime.Serialization;
using HLS.Topup.Common;

namespace HLS.Topup.Dtos.Stock
{
    public class StockResponseDto
    {
        public string Id { get; set; }
        public string StockCode { get; set; } 
        public int Inventory { get; set; }
        public int InventoryLimit { get; set; }
        public int MinimumInventoryLimit { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        [DataMember(Name = "itemValue")]
        public decimal CardValue { get; set; }
        [DataMember(Name = "keyCode")]
        public string ProductCode { get; set; } 
        public string ProductName { get; set; } 
        // dịch vu
        public string ServiceName { get; set; }
        public string ServiceCode { get; set; }
        // loai sp
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }

    }
}
