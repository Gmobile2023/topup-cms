using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.AccountManagement;
using HLS.Topup.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Web.Models.Account;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_SubAgentManagement)]
    public class AgentManagementController : TopupControllerBase
    {
        private readonly UserManager _userManager;
        private readonly TopupAppSession _topupAppSession;
        private readonly IAccountManagementAppService _accountManagementAppService;

        public AgentManagementController(UserManager userManager, TopupAppSession topupAppSession,
            IAccountManagementAppService accountManagementAppService)
        {
            _userManager = userManager;
            _topupAppSession = topupAppSession;
            _accountManagementAppService = accountManagementAppService;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_SubAgentManagement_Create, AppPermissions.Pages_SubAgentManagement_Edit)]
        public async Task<IActionResult> Detail(long? userId, bool isView = false)
        {
            try
            {
                var modal = new SubAgentManagementDto();
                modal.AccountInfo = new UserProfileDto();
                if (userId.HasValue)
                    modal.AccountInfo = await _accountManagementAppService.GetAccount(new EntityDto<long>(userId ?? 0));
                else
                    modal.AccountInfo.IsActive = true;
                modal.IsViewMode = isView;
                modal.IsEditMode = userId != null;
                return View(modal);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
