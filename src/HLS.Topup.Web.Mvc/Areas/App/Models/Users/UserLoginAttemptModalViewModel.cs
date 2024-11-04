using System.Collections.Generic;
using HLS.Topup.Authorization.Users.Dto;

namespace HLS.Topup.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}