using HLS.Topup.Sale.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;
using Abp.Extensions;
using HLS.Topup.Dtos.Sale;

namespace HLS.Topup.Web.Areas.App.Models.SaleMans
{
    public class CreateOrEditSaleManModalViewModel
    {
        public CreateOrUpdateSaleDto SaleMan { get; set; }
        public List<SaleManUserLookupTableDto> SaleManUserList { get; set; }

        //public List<AddressSaleItemDto> SaleManCityList { get; set; }
        public bool IsEditMode => SaleMan.Id.HasValue;
    }
}
