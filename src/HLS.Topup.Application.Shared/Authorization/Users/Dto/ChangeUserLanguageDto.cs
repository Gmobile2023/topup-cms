using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
