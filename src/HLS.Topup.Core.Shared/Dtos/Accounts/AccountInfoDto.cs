using HLS.Topup.Authorization.Users;

namespace HLS.Topup.Dtos.Accounts
{
    public class UserAccountInfoDto
    {
        public UserInfoDto NetworkInfo { get; set; }
        public UserInfoDto UserInfo { get; set; }
    }
}
