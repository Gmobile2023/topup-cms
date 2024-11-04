using HLS.Topup.BalanceManager.Dtos;
using System.Collections.Generic;

using Abp.Extensions;
using Abp.Application.Services.Dto;

namespace HLS.Topup.Web.Areas.App.Models.PayBatchBills
{
    public class CreateOrEditPayBatchBillModalViewModel
    {
        public CreateOrEditPayBatchBillDto PayBatchBill { get; set; }

        public string ProductProductName { get; set; }

        public List<ComboboxItemDto> Categorys { get; set; }

        public List<PayBatchBillProductLookupTableDto> PayBatchBillProductList { get; set; }

        public bool IsEditMode => PayBatchBill.Id.HasValue;
    }
}