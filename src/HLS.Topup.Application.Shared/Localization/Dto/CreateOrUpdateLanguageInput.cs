using System.ComponentModel.DataAnnotations;

namespace HLS.Topup.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}