using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using HLS.Topup.Common;
using HLS.Topup.Dtos.Discounts;
using HLS.Topup.Validation;

namespace HLS.Topup.Authorization.Users
{
    public class CreateAccountDto : IValidatableObject
    {
        public int? Id { get; set; }

        /// <summary>
        /// Tên
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// Họ
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        //[Required]
        //[StringLength(AbpUserBase.MaxUserNameLength)]
        //public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// Mật khẩu
        /// </summary>
        //[Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        /// <summary>
        /// Email không bắt buộc
        /// </summary>
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public bool IsEmailConfirmed { get; set; }
        public string EmailActivationLink { get; set; }
        public string ParentAccount { get; set; }
        public CommonConst.Channel Channel { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public bool IsActive { get; set; }
        public bool IsCheckStatus { get; set; }
        public bool IsVerifyAccount { get; set; }
        public string AccountCodeRef { get; set; }
        public string AgentName { get; set; }
        public string AccountCode { get; set; }
        public int?TenantId { get; set; }
        public string Address { get; set; }
        public int? CityId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string Description { get; set; }
        //public bool IsUserStaff { get; set; }
        public byte? Gender { get; set; }
        public DateTime? DoB { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!PhoneNumber.IsNullOrEmpty())
            {
                if (!ValidationHelper.IsPhone(PhoneNumber))
                {
                    yield return new ValidationResult(("PhoneNumber invalid!"));
                }
            }

            // if (!ParentAccount.IsNullOrEmpty())
            // {
            //     if (!ValidationHelper.IsPhone(ParentAccount))
            //     {
            //         yield return new ValidationResult("SponsorMobile invalid!");
            //     }
            // }
        }

        public string FrontPhoto { get; set; }
        public string BackSitePhoto { get; set; }
        public string IdIdentity { get; set; }
        public CommonConst.IdType? IdType { get; set; }
        public DateTime? IdentityIdExpireDate { get; set; }
        public DateTime? SigDate { get; set; }
        public string ContractNumber { get; set; }
        public DateTime? ContractRegister { get; set; }

        public CommonConst.MethodReceivePassFile? MethodReceivePassFile { get; set; }

        public string ValueReceivePassFile { get; set; }
    }

    public class UserInfoDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public byte? Gender { get; set; }
        public DateTime? DoB { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsActive { get; set; }
        public string AccountCode { get; set; }
        public string ParentCode { get; set; }
        public string TreePath { get; set; }
        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public int NetworkLevel { get; set; }
        public long OrganizationUnitId { get; set; }
        public string AgentName { get; set; }
        public bool IsVerifyAccount { get; set; }
        public long? UserSaleLeadId { get; set; }
        public string SaleCode { get; set; }
        public string LeaderCode { get; set; }
        public long? ParentId { get; set; }
        public UserUnit Unit { get; set; }
    }

    public class UserInfoPeriodDto
    {
        public long Id { get; set; }
        public string AgentCode { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        public int Period { get; set; }

        public string ContractNumber { get; set; }
        public string EmailReceives { get; set; }
        public string FolderFtp { get; set; }

        public DateTime? SigDate { get; set; }
    }

    public class UserUnit
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DistrictCode { get; set; }
        public int WardId { get; set; }
        public string WardName { get; set; }
        public string WardCode { get; set; }
        public string IdIdentity { get; set; }
        public string ChatId { get; set; }
    }

    public class UserInputDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserInfoSaleDto
    {
        public string SaleCode { get; set; }
        public string SaleLeaderCode { get; set; }
        public long UserSaleId { get; set; }
        public long UserLeaderId { get; set; }
    }

    public class UpdateDiscountAccountDto
    {
        public List<DiscountDetailDto> DiscountDetail { get; set; }
        public long UserId { get; set; }
    }
    public class UserLimitDebtDto
    {
        public decimal Limit { get; set; }

        public int DebtAge { get; set; }
    }
}
