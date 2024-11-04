using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Authorization;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Authorization.Users;
using HLS.Topup.PostManagement;
using HLS.Topup.PostManagement.Dtos;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_PostManagement)]
    public class PostManagementController : TopupControllerBase
    {
        private readonly UserManager _userManager;
        private readonly TopupAppSession _topupAppSession;
        private readonly IPostManagementAppService _postManagement;

        public PostManagementController(UserManager userManager, TopupAppSession topupAppSession,
            IPostManagementAppService postManagement)
        {
            _userManager = userManager;
            _topupAppSession = topupAppSession;
            _postManagement = postManagement;
        }


        public async Task<IActionResult> Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_PostManagement_Create, AppPermissions.Pages_PostManagement_Edit)]
        public async Task<IActionResult> Detail(long? userId, bool isView = false)
        {
            try
            {
                var modal = new PostManagementDto();
                if (userId.HasValue)
                    modal = await _postManagement.GetAgentDetail(userId.Value);

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
