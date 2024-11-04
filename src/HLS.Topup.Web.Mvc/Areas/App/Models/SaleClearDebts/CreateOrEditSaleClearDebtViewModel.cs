using HLS.Topup.Sale.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.SaleClearDebts
{
    public class CreateOrEditSaleClearDebtModalViewModel
    {
       public CreateOrEditSaleClearDebtDto SaleClearDebt { get; set; }

	   		public string UserName { get; set;}

		public string BankBankName { get; set;}


       public List<SaleClearDebtBankLookupTableDto> SaleClearDebtBankList { get; set;}


	   public bool IsEditMode => SaleClearDebt.Id.HasValue;
    }
}