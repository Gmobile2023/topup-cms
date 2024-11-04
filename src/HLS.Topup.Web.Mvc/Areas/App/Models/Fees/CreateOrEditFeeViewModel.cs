using HLS.Topup.FeeManager.Dtos;
using System.Collections.Generic;
using Abp.Extensions;
using HLS.Topup.Dtos.BillFees;

namespace HLS.Topup.Web.Areas.App.Models.Fees
{
    public class CreateOrEditFeeModalViewModel
    {
        public CreateOrEditFeeDto Fee { get; set; }

        public string UserName { get; set; }
        
        public List<FeeUserLookupTableDto> FeeUserList { get; set; }
        
        public List<FeeLookupTableDto> FeeCategoryList { get; set; }

        public bool IsEditMode => Fee.Id.HasValue;
        
        public bool IsViewMode { get; set; }
        
        public List<BillFeeDetailDto> ProductFees { get; set; }
    }
}