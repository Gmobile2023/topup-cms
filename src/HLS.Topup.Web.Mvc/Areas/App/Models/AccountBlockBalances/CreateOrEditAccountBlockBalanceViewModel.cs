using HLS.Topup.BalanceManager.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.AccountBlockBalances
{
    public class CreateOrEditAccountBlockBalanceModalViewModel
    {
       public CreateOrEditAccountBlockBalanceDto AccountBlockBalance { get; set; }

	   		public string UserName { get; set;}


       public List<AccountBlockBalanceUserLookupTableDto> AccountBlockBalanceUserList { get; set;}


	   public bool IsEditMode => AccountBlockBalance.Id.HasValue;
    }
}