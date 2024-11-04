using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.AspNetZeroCore.Validation;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using HLS.Topup.Authorization.Users;
using HLS.Topup.Security;

namespace HLS.Topup.Web.Models.Account
{
    public class RegisterViewModel //: IValidatableObject
    {
        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string Surname { get; set; }

        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        //[Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        //[StringLength(10)] public string PhoneNumber { get; set; }

        [StringLength(User.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        public bool IsExternalLogin { get; set; }

        public string ExternalLoginAuthSchema { get; set; }

        public string ReturnUrl { get; set; }

        public string SingleSignIn { get; set; }
        public string Otp { get; set; }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        // if (!UserName.IsNullOrEmpty())
        // {
        //     if (!UserName.Equals(EmailAddress, StringComparison.OrdinalIgnoreCase) &&
        //         ValidationHelper.IsEmail(UserName))
        //     {
        //         yield return new ValidationResult(
        //             "Email không được trùng với email đã đăng ký !");
        //     }
        // }

        // if (!PhoneNumber.IsNullOrEmpty())
        // {
        //     if (!Validation.ValidationHelper.IsPhone(PhoneNumber))
        //     {
        //         yield return new ValidationResult(
        //             "Số điện thoại không hợp lệ");
        //     }
        // }
        //}
    }
}
