using System.Collections.Generic;
using HLS.Topup.Authorization.Permissions.Dto;

namespace HLS.Topup.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}