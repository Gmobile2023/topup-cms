using HLS.Topup.BalanceManager.Dtos;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.SystemAccountTransfers
{
    public class CreateOrEditSystemAccountTransferModalViewModel
    {
       public CreateOrEditSystemAccountTransferDto SystemAccountTransfer { get; set; }

	   
       
	   public bool IsEditMode => SystemAccountTransfer.Id.HasValue;
    }
}