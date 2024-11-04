using System;
using System.Web;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;
using HLS.Topup.Authorization.Accounts.Dto;
using HLS.Topup.Security;

namespace HLS.Topup.Web.Models.Account
{
    public class ResetPasswordViewModel : ResetPasswordInput
    {
        public int? TenantId { get; set; }
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
        public bool IsObpEnable { get; set; }
        public bool IsUseVerify { get; set; }
        public string Message { get; set; }

        protected override void ResolveParameters()
        {
            base.ResolveParameters();

            if (!string.IsNullOrEmpty(c))
            {
                var parameters = SimpleStringCipher.Instance.Decrypt(c);
                var query = HttpUtility.ParseQueryString(parameters);

                if (query["tenantId"] != null && !string.IsNullOrEmpty(query["tenantId"]))
                {
                    TenantId = Convert.ToInt32(query["tenantId"]);
                }
            }
        }
    }
}
