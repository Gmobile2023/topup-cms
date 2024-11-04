using System.Collections.Generic;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Authorization.Users.Dto;
using HLS.Topup.Authorization.Users.Profile.Dto;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Security;

namespace HLS.Topup.Authorization.Accounts.Dto
{
    public class CreateOrEditAgentModalViewModel
    {
        public bool CanChangeUserName
        {
            get { return User.UserName != "Admin"; }
        }

        public bool IsEditMode
        {
            get { return User.Id.HasValue; }
        }

        public UserEditDto User { get; set; }
        //public List<DiscountDetailDto> DiscountDetail { get; set; }
        public int? DiscountId { get; set; }
        public bool IsView { get; set; }
        public UserProfileDto AgentProfile { get; set; }
        
       
    }

    public class BalanceAccountInfo
    {
        public decimal Balance { get; set; }
        public decimal DiscountTotal { get; set; }
    }

    public class ChangePasswordUserDto: ChangePasswordInput
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
    }
}