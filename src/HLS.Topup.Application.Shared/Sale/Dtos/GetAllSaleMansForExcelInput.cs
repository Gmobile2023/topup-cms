using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Sale.Dtos
{
    public class GetAllSaleMansForExcelInput
    {
        public string Filter { get; set; }

        public string SaleCodeFilter { get; set; }

        public string SaleNameFilter { get; set; }

        public int? SaleTypeFilter { get; set; }

        public int? StatusFilter { get; set; }
        public long? SaleLeadId { get; set; }


        public string UserNameFilter { get; set; }
        
    }
}
