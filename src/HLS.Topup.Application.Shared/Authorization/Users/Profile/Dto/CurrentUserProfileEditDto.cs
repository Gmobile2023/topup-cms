using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Configuration;

namespace HLS.Topup.Authorization.Users.Profile.Dto
{
    public class CurrentUserProfileEditDto
    {
        public  long Id { get; set; }
        //[Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        //[Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        //[Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        //[Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        [StringLength(UserConsts.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }

        public virtual bool IsPhoneNumberConfirmed { get; set; }

        public string Timezone { get; set; }

        public string QrCodeSetupImageUrl { get; set; }

        public bool IsGoogleAuthenticatorEnabled { get; set; }
        public string Password { get; set; }
        public string CurrentPassword { get; set; }

        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public bool IsActive { get; set; }
        public bool IsUpdateVerify { get; set; }
        public bool IsVerifyAccount { get; set; }

        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public CommonConst.IdType? IdType { get; set; } //Loại giấy tờ
        public string IdentityId { get; set; } //Số giấy tờ
        public string FrontPhoto { get; set; }
        public string BackSitePhoto { get; set; }
        public string AgentName { get; set; }
        public string AgentAddress { get; set; }
        public string AgentCode { get; set; }
        public bool AgentIsVerify { get; set; }
        public bool AgentIsActive { get; set; }
        public string ExtraInfo { get; set; }
        public string AccountCode { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
        public StaffInfo StaffInfo { get; set; }
    }

    public class UpdateProfileInfoRequest
    {
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }
        //[Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string AgentName { get; set; }

    }

    public class StaffInfo
    {
        public decimal LimitAmount { get; set; }
        public decimal LimitPerTrans { get; set; }
        public string Description { get; set; }
        public List<int> Days { get; set; }
        public StaffTime FromTime { get; set; }
        public StaffTime ToTime { get; set; }
    }

    public class UpdateProfileLimitDto
    {
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        public string Timezone { get; set; }

        public string QrCodeSetupImageUrl { get; set; }

        public bool IsGoogleAuthenticatorEnabled { get; set; }
        public string Password { get; set; }
        public string CurrentPassword { get; set; }

        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public bool IsActive { get; set; }
        public bool IsUpdateVerify { get; set; }
        public bool IsVerifyAccount { get; set; }

        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public CommonConst.IdType? IdType { get; set; } //Loại giấy tờ
        public string IdentityId { get; set; } //Số giấy tờ
        public string FrontPhoto { get; set; }
        public string BackSitePhoto { get; set; }
        public string AgentName { get; set; }
        public string ExtraInfo { get; set; }
        public string AccountCode { get; set; }
        public string FullName { get; set; }
        public StaffInfo StaffInfo { get; set; }
        public DateTime? IdentityIdExpireDate { get; set; }
    }
}
