using System.Collections.Generic;
using Abp.Application.Services.Dto;
using HLS.Topup.Authorization.Permissions.Dto;
using HLS.Topup.Web.Areas.App.Models.Common;

namespace HLS.Topup.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}