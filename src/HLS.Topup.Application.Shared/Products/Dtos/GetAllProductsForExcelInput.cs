using Abp.Application.Services.Dto;
using System;

namespace HLS.Topup.Products.Dtos
{
    public class GetAllProductsForExcelInput
    {
        public string Filter { get; set; }

        public string ProductCodeFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public int? ProductTypeFilter { get; set; }

        public int? StatusFilter { get; set; }

        public string UnitFilter { get; set; }
        
        public string CategoryCategoryNameFilter { get; set; }
        
        public string CustomerSupportNoteFilter { get; set; }
        
        public string UserManualNoteFilter { get; set; }
    }
}