using Abp.Authorization;
using HLS.Topup.Authorization.Roles;
using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
