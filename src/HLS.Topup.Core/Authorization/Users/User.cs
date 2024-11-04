using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using Abp.Timing;
using HLS.Topup.Common;

namespace HLS.Topup.Authorization.Users
{
    /// <summary>
    /// Represents a user in the system.
    /// </summary>
    public class User : AbpUser<User>
    {
        public virtual Guid? ProfilePictureId { get; set; }

        public virtual bool ShouldChangePasswordOnNextLogin { get; set; }

        public DateTime? SignInTokenExpireTimeUtc { get; set; }

        public string SignInToken { get; set; }

        public string GoogleAuthenticatorKey { get; set; }

        public List<UserOrganizationUnit> OrganizationUnits { get; set; }

        //Can add application specific user properties here

        public User()
        {
            IsLockoutEnabled = true;
            IsTwoFactorEnabled = true;
        }

        /// <summary>
        /// Creates admin <see cref="User"/> for a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant Id</param>
        /// <param name="emailAddress">Email address</param>
        /// <returns>Created <see cref="User"/> object</returns>
        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>(),
                OrganizationUnits = new List<UserOrganizationUnit>()
            };

            user.SetNormalizedNames();

            return user;
        }

        public override void SetNewPasswordResetCode()
        {
            /* This reset code is intentionally kept short.
             * It should be short and easy to enter in a mobile application, where user can not click a link.
             */
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

        public void Unlock()
        {
            AccessFailedCount = 0;
            LockoutEndDateUtc = null;
        }

        public void Block()
        {
            IsActive = false;
        }

        public void Unblock()
        {
            IsActive = true;
        }

        public void SetSignInToken()
        {
            SignInToken = Guid.NewGuid().ToString();
            SignInTokenExpireTimeUtc = Clock.Now.AddMinutes(1).ToUniversalTime();
        }

        public string GenAccountCode()
        {
            var random = new Random().Next(10000, 99999);
            switch (AccountType)
            {
                case CommonConst.SystemAccountType.Agent:
                case CommonConst.SystemAccountType.MasterAgent:
                case CommonConst.SystemAccountType.Company:
                    return "NT9" + random;
                case CommonConst.SystemAccountType.Staff:
                    return "NT9" + random + Id;
                case CommonConst.SystemAccountType.StaffApi:
                    return "NT9" + random + Id;
                case CommonConst.SystemAccountType.Sale:
                    return "S" + random + Id;
                case CommonConst.SystemAccountType.SaleLead:
                    return "SL" + random + Id;
                case CommonConst.SystemAccountType.System:
                    break;
                default:
                    return "A" + Id.ToString("000000000000");
            }

            return "A" + Id.ToString("000000000000");
        }

        public bool IsAccountSystem()
        {
            return AccountType == CommonConst.SystemAccountType.System ||
                   AccountType == CommonConst.SystemAccountType.Sale ||
                   AccountType == CommonConst.SystemAccountType.SaleLead ||
                   AccountType == CommonConst.SystemAccountType.StaffApi ||
                   AccountType == CommonConst.SystemAccountType.Staff;
        }


        public override string FullName => Surname + " " + Name;

        //[Required]
        [Column(TypeName = "varchar(50)")] public string AccountCode { get; set; }
        [Column(TypeName = "varchar(50)")] public string ParentCode { get; set; }

        [Column(TypeName = "varchar(20)")] public string MobileOtp { get; set; }

        //[Required]
        [Column(TypeName = "varchar(500)")] public string TreePath { get; set; }
        public long? ParentId { get; set; }

        [Column(TypeName = "varchar(255)")] public string Level2Password { get; set; }

        public int? AccessFailedCountLevel2Pass { get; set; }
        [Column(TypeName = "nvarchar(500)")] public string Description { get; set; }

        public DateTime? LockTransDateUtc { get; set; }

        public CommonConst.SystemAccountType AccountType { get; set; }
        public CommonConst.AgentType AgentType { get; set; }
        //public bool IsUserStaff { get; set; }

        public DateTime? DoB { get; set; }

        public byte? Gender { get; set; }

        public int NetworkLevel { get; set; }
        public bool IsDefaultEmail { get; set; }
        public bool IsVerifyAccount { get; set; }

        [StringLength(255)] public string AgentName { get; set; }

        public virtual long? UserSaleLeadId { get; set; }
        [Column(TypeName = "nvarchar(255)")]
        public string LockAccountNote { get; set; }

        [ForeignKey("UserSaleLeadId")] public User UserSaleLeadFk { get; set; }
    }
}
