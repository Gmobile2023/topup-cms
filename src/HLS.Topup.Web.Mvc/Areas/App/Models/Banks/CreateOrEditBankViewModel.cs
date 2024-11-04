using HLS.Topup.Banks.Dtos;

using Abp.Extensions;

namespace HLS.Topup.Web.Areas.App.Models.Banks
{
    public class CreateOrEditBankModalViewModel
    {
       public CreateOrEditBankDto Bank { get; set; }
       
	   public bool IsEditMode => Bank.Id.HasValue;
    }
}