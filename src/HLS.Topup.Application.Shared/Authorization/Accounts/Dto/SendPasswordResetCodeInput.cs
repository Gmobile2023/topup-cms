using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace HLS.Topup.Authorization.Accounts.Dto
{
    public class SendPasswordResetCodeInput
    {
        //[Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class SendResetCodeInput
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string ConfirmCode { get; set; }
    }
}
