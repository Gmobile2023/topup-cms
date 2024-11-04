using HLS.Topup.Sale.Dtos;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.SaleLimitDebts
{
    public class CreateOrEditSaleLimitDebtModalViewModel
    {
       public CreateOrEditSaleLimitDebtDto SaleLimitDebt { get; set; }

	   		public string UserName { get; set;}


       
	   public bool IsEditMode => SaleLimitDebt.Id.HasValue;
    }
}