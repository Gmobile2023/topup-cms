using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.UI;
using HLS.Topup.Authorization;
using HLS.Topup.BalanceManager;
using HLS.Topup.BalanceManager.Dtos;
using HLS.Topup.Web.Areas.App.Models.LowBalanceAlerts;
using HLS.Topup.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_LowBalanceAlerts)]
    public class LowBalanceAlertsController : TopupControllerBase
    {
        private readonly ILowBalanceAlertsAppService _lowBalanceAlertsAppService;
        
        public LowBalanceAlertsController(ILowBalanceAlertsAppService lowBalanceAlertsAppService)
        {
            _lowBalanceAlertsAppService = lowBalanceAlertsAppService;
        }
        
        public ActionResult Index()
        {
            var model = new BalanceAlertsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }
        
        [AbpMvcAuthorize(AppPermissions.Pages_LowBalanceAlerts_Create, AppPermissions.Pages_LowBalanceAlerts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(string accountCode)
        {
            GetLowBalanceAlertForEditOutput getLowBalanceAlertForEditOutput;

            if (accountCode != null)
            {
                getLowBalanceAlertForEditOutput = await _lowBalanceAlertsAppService.GetLowBalanceAlertForEdit(accountCode);
                
                if (getLowBalanceAlertForEditOutput.LowBalanceAlert.Id == null)
                    throw new UserFriendlyException("Không lấy được thông tin cấu hình. Vui lòng thử lại sau");
            }
            else
            {
                getLowBalanceAlertForEditOutput = new GetLowBalanceAlertForEditOutput
                {
                    LowBalanceAlert = new CreateOrEditLowBalanceAlertDto()
                };
            }

            var viewModel = new CreateOrEditLowBalanceAlertModalViewModel()
            {
                LowBalanceAlert = getLowBalanceAlertForEditOutput.LowBalanceAlert,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }
        
        public async Task<PartialViewResult> ViewBalanceAlertModal(string accountCode)
        {
            var getLowBalanceAlertForViewDto = await _lowBalanceAlertsAppService.GetLowBalanceAlertForView(accountCode);
        
            var model = new BalanceAlertViewModel()
            {
                LowBalanceAlert = getLowBalanceAlertForViewDto.LowBalanceAlert
            };
        
            return PartialView("_ViewLowBalanceAlertModal", model);
        }
    }
}