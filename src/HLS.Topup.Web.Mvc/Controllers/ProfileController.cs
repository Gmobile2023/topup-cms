using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Abp;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.AspNetZeroCore.Net;
using Abp.Auditing;
using Abp.Domain.Uow;
using Abp.IO.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Web.Models;
using HLS.Topup.Authorization;
using HLS.Topup.Authorization.Accounts.Dto;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Authorization.Users.Profile;
using HLS.Topup.Authorization.Users.Profile.Dto;
using HLS.Topup.Common;
using HLS.Topup.DemoUiComponents.Dto;
using HLS.Topup.DiscountManager;
using HLS.Topup.Dtos.Accounts;
using HLS.Topup.Dtos.Common;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Friendships;
using HLS.Topup.Security;
using HLS.Topup.Storage;
using HLS.Topup.Web.Models.Account;
using HLS.Topup.Web.TagHelpers;
using ServiceStack;
using StringExtensions = Abp.Extensions.StringExtensions;

namespace HLS.Topup.Web.Controllers
{
    [AbpMvcAuthorize]
    [DisableAuditing]
    public class ProfileController : ProfileControllerBase
    {
        private readonly IProfileAppService _profileAppService;
        private readonly UserManager _userManager;
        private readonly TopupAppSession _topupAppSession;

        public ProfileController(
            ITempFileCacheManager tempFileCacheManager,
            IProfileAppService profileAppService, UserManager userManager,
            TopupAppSession topupAppSession) : base(
            tempFileCacheManager, profileAppService)
        {
            _profileAppService = profileAppService;
            _userManager = userManager;
            _topupAppSession = topupAppSession;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            var userInfo = user.ConvertTo<UserEditDto>();
            var model = new ProfileModel
            {
                //Todo Comment chỗ discount này. Hiện tại UI mới k show discount của TK
                Disouncts =
                    new List<ProductDiscountDto>(), //await _discountManger.GetProductDiscounts(null, GetAccountInfo().NetworkInfo.AccountCode),
                UserInfo = userInfo,
                UserAccountInfo = new UserAccountInfoDto()
            };
            try
            {
                model.IsLevel2Password = await _profileAppService.CheckLevel2PasswordAsync();
            }
            catch (Exception e)
            {
                model.IsLevel2Password = false;
            }

            try
            {
                model.AgentProfile = await _userManager.GetAgentProfile(user.Id);
                if (_topupAppSession.AccountType == CommonConst.SystemAccountType.StaffApi)
                {
                    model.UserAccountInfo = _userManager.GetAccountInfo();
                }
            }
            catch (Exception e)
            {
                model.AgentProfile = new UserProfileDto();
            }

            return View(model);
        }

        public async Task<IActionResult> Edit()
        {
            try
            {
                var user = await _userManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
                var output = user.ConvertTo<UserEditDto>();
                var viewModel = new CreateOrEditAgentModalViewModel()
                {
                    User = output,
                    AgentProfile = await _userManager.GetAgentProfile(user.Id)
                };
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        public async Task<IActionResult> Password2Level()
        {
            try
            {
                ViewBag.IsLevel2Password = await _profileAppService.CheckLevel2PasswordAsync();
            }
            catch (Exception e)
            {
                ViewBag.IsLevel2Password = false;
            }

            return View();
        }


        public async Task<FileResult> GetProfilePicture()
        {
            var output = await _profileAppService.GetProfilePicture();

            if (StringExtensions.IsNullOrEmpty(output.ProfilePicture))
            {
                return GetDefaultProfilePictureInternal();
            }

            return File(Convert.FromBase64String(output.ProfilePicture), MimeTypeNames.ImageJpeg);
        }

        //Gunner xem lại [UnitOfWork] sau khi nâng cấp lên abp mới nhất
        public virtual async Task<FileResult> GetFriendProfilePicture(long userId, int? tenantId)
        {
            var output = await _profileAppService.GetFriendProfilePicture(new GetFriendProfilePictureInput()
            {
                TenantId = tenantId,
                UserId = userId
            });

            if (StringExtensions.IsNullOrEmpty(output.ProfilePicture))
            {
                return GetDefaultProfilePictureInternal();
            }

            return File(Convert.FromBase64String(output.ProfilePicture), MimeTypeNames.ImageJpeg);
        }

        public async Task<PartialViewResult> CreateOrEditModal()
        {
            try
            {
                var user = await _userManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
                var output = user.ConvertTo<UserEditDto>();
                var viewModel = new CreateOrEditAgentModalViewModel()
                {
                    User = output
                };
                return PartialView("_CreateOrEditModal", viewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // fix để client ko gọi update trực tiếp vào API
        // chi cho phép update 1 số thông tin
        public async Task<JsonResult> UpdateCurrentUserProfile(UserEditProfileModel model)
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            var m = model.ConvertTo<CurrentUserProfileEditDto>();
            m.UserName = user.UserName;
            m.EmailAddress = user.EmailAddress;
            m.PhoneNumber = user.PhoneNumber;
            if (string.IsNullOrEmpty(m.FrontPhoto))
                m.FrontPhoto = model.url_before;
            if (string.IsNullOrEmpty(m.BackSitePhoto))
                m.BackSitePhoto = model.url_after;
            await _profileAppService.UpdateCurrentUserProfile(m);
            return Json(new AjaxResponse() { });
        }

        public async Task ChangePasswordCurrentUserProfile(UserChangepasswordProfileModel model)
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId ?? 0);
            var m = new CurrentUserProfileEditDto()
            {
                UserName = user.UserName,
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
                Surname = user.Surname,
                Name = user.Name,
                Password = model.PasswordNew,
                CurrentPassword = model.Password
            };
            await _profileAppService.UpdateCurrentUserProfile(m);
        }


        public PartialViewResult Level2PasswordModal()
        {
            return PartialView("_Level2PasswordModal");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_StaffManager)]
        public IActionResult StaffManager()
        {
            return View("StaffManager");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_StaffManager_Create)]
        public IActionResult CreateStaff()
        {
            ViewBag.isEdit = false;
            return View("CreateOrEditStaff");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_StaffManager_Create)]
        [HttpGet]
        public IActionResult EditStaff(string id)
        {
            ViewBag.isEdit = true;
            ViewBag.staffId = id;
            return View("CreateOrEditStaff");
        }

        public IActionResult SalesPolicy()
        {
            return View("SalesPolicy");
        }

        public IActionResult Notifications()
        {
            return View();
        }

        public IActionResult SecurityMethod()
        {
            var method = _profileAppService.GetPaymentVerifyMethod(new GetPaymentMothodInput
                {Channel = CommonConst.Channel.WEB});
            var model = new PaymentVerifyTransTypeModel()
            {
                Method = method.Result
            };

            return View("SecurityMethod", model);
        }
    }
}
