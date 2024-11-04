using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using HLS.Topup.Address;
using Microsoft.AspNetCore.Mvc;
using HLS.Topup.Authorization.Permissions;
using HLS.Topup.Authorization.Permissions.Dto;
using HLS.Topup.Web.Areas.App.Models.Common.Modals;
using HLS.Topup.Web.Controllers;
using HLS.Topup.Web.TagHelpers;
using ServiceStack;

namespace HLS.Topup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize]
    public class CommonController : TopupControllerBase
    {
        private readonly IPermissionAppService _permissionAppService;
        private readonly IDistrictsAppService _districtAppService;
        private readonly IViewRender _viewRender;

        public CommonController(IPermissionAppService permissionAppService, IViewRender viewRender, IDistrictsAppService districtAppService)
        {
            _permissionAppService = permissionAppService;
            _viewRender = viewRender;
            _districtAppService = districtAppService;
        }

        public PartialViewResult LookupModal(LookupModalViewModel model)
        {
            return PartialView("Modals/_LookupModal", model);
        }

        public PartialViewResult EntityTypeHistoryModal(EntityHistoryModalViewModel input)
        {
            return PartialView("Modals/_EntityTypeHistoryModal", ObjectMapper.Map<EntityHistoryModalViewModel>(input));
        }

        public PartialViewResult PermissionTreeModal(List<string> grantedPermissionNames = null)
        {
            var permissions = _permissionAppService.GetAllPermissions().Items.ToList();

            var model = new PermissionTreeModalViewModel
            {
                Permissions = ObjectMapper.Map<List<FlatPermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList(),
                GrantedPermissionNames = grantedPermissionNames
            };

            return PartialView("Modals/_PermissionTreeModal", model);
        }

        public PartialViewResult InactivityControllerNotifyModal()
        {
            return PartialView("Modals/_InactivityControllerNotifyModal");
        }
    }
}