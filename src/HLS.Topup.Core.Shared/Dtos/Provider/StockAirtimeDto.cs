using System.Runtime.Serialization;
using HLS.Topup.StockManagement;

namespace HLS.Topup.Dtos.Provider
{
    public class StocksAirtimeDto
    {
        public int Id { get; set; }

        // public string StockCode { get; set; }
        // public string StockType { get; set; }
        public string KeyCode { get; set; }
        public string ProviderCode { get; set; }
        public string ProviderName { get; set; }
        [DataMember(Name = "inventory")] public decimal TotalAirtime { get; set; }
        public decimal TotalAmount { get; set; }
        public StocksAirtimeStatus Status { get; set; }
        public string Description { get; set; }

        [DataMember(Name = "minimumInventoryLimit")]
        public decimal MinLimitAirtime { get; set; }

        [DataMember(Name = "inventoryLimit")] public decimal MaxLimitAirtime { get; set; }
    }
}
