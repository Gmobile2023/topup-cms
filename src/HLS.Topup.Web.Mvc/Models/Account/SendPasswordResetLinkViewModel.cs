using System.ComponentModel.DataAnnotations;
using HLS.Topup.Security;

namespace HLS.Topup.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
    public class SendOtpResetPassViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }
    }
    public class ResetPassViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ResetCode { get; set; }
        public long UserId { get; set; }
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }
    }
}
