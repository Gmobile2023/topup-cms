using System;
using Abp;

namespace HLS.Topup.StockManagement.Dtos
{
    public class ImportCardsFromExcelJobArgs
    {
        public int? TenantId { get; set; }

        public Guid BinaryObjectId { get; set; }

        public UserIdentifier User { get; set; }

        public string StockType { get; set; }
        public string BatchCode { get; set; }
        public int CardValue { get; set; }
    }
}