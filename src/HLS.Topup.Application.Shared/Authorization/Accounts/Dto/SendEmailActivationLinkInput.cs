using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}