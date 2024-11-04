using HLS.Topup.BalanceManager.Dtos;

namespace HLS.Topup.Web.Areas.App.Models.LowBalanceAlerts
{
    public class CreateOrEditLowBalanceAlertModalViewModel
    {
        public CreateOrEditLowBalanceAlertDto LowBalanceAlert { get; set; }
        public bool IsEditMode => LowBalanceAlert.Id != null;
    }
}