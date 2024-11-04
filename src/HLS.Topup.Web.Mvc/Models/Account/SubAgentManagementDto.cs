using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Web.Models.Account
{
    public class SubAgentManagementDto
    {
        public UserProfileDto AccountInfo { get; set; }
        public bool IsEditMode { get; set; }
        public bool IsViewMode { get; set; }
    }
}
