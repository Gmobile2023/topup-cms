using System.Collections.Generic;
using HLS.Topup.Authorization.Permissions.Dto;

namespace HLS.Topup.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}