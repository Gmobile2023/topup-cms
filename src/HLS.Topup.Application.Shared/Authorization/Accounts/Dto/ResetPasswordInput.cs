using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using Abp.Auditing;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;
using HLS.Topup.Common;

namespace HLS.Topup.Authorization.Accounts.Dto
{
    public class ResetPasswordInput: IShouldNormalize
    {
        public long UserId { get; set; }
        public bool IsVerify { get; set; }
        //public long ConfirmId { get; set; }

        public string ResetCode { get; set; }

        [DisableAuditing]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
        public string Otp { get; set; }
        public CommonConst.Channel Channel { get; set; }

        public string SingleSignIn { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Encrypted values for {TenantId}, {UserId} and {ResetCode}
        /// </summary>
        public string c { get; set; }

        public void Normalize()
        {
            ResolveParameters();
        }

        protected virtual void ResolveParameters()
        {
            if (!string.IsNullOrEmpty(c))
            {
                var parameters = SimpleStringCipher.Instance.Decrypt(c);
                var query = HttpUtility.ParseQueryString(parameters);

                if (query["userId"] != null)
                {
                    UserId = Convert.ToInt32(query["userId"]);
                }

                if (query["resetCode"] != null)
                {
                    ResetCode = query["resetCode"];
                }
            }
        }
    }
}
