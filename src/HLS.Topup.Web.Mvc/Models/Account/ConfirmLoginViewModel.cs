using System;
using Abp.Runtime.Security;
using Abp.Runtime.Validation;
using ServiceStack.Pcl;

namespace HLS.Topup.Web.Models.Account
{
    public class ConfirmLoginViewModel : IShouldNormalize
    {
        public int? TenantId { get; set; }
        public long ConfirmId { get; set; }
        public string PhoneNumber { get; set; }
        public string ReturnUrl { get; set; }
        public string SingleSignIn { get; set; }
        public bool IsOdpEnable { get; set; }
        public string Message { get; set; }
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

                if (query["ConfirmId"] != null)
                {
                    ConfirmId = Convert.ToInt32(query["ConfirmId"]);
                }

                if (query["PhoneNumber"] != null)
                {
                    PhoneNumber = query["PhoneNumber"];
                }
                if (query["ReturnUrl"] != null)
                {
                    ReturnUrl = query["ReturnUrl"];
                }
                if (query["TenantId"] != null && !string.IsNullOrEmpty(query["TenantId"]) && query["TenantId"]!="0")
                {
                    TenantId = int.Parse(query["TenantId"]);
                }
                if (query["SingleSignIn"] != null)
                {
                    SingleSignIn = query["SingleSignIn"];
                }
            }
        }
    }

    public class ConfirmLoginInputModel : ConfirmLoginViewModel
    {
        public string Otp { get; set; }
    }
}
