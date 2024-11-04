using HLS.Topup.Deposits.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Deposits
{
    public class CreateOrEditDepositModalViewModel
    {
        public CreateOrEditDepositDto Deposit { get; set; }

        public string UserName { get; set; }
        
        public string BankId { get; set; }

        public string BankBankName { get; set; }

        public string UserName2 { get; set; }

        public string UserNameSale { get; set; }

        public double SaleLimit { get; set; }

        public List<DepositBankLookupTableDto> DepositBankList { get; set; }

        public List<DepositUserLookupTableDto> DepositUserList { get; set; }

        public bool IsEditMode => Deposit.Id.HasValue;
    }

    public class ApprovedBankModalViewModel
    {
        public string TransCode { get; set; }
    }
}