using System.Collections.Generic;
using Abp.Localization;
using HLS.Topup.Install.Dto;

namespace HLS.Topup.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
