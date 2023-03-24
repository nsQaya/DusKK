using System.Collections.Generic;
using Abp.Localization;
using TDV.Install.Dto;

namespace TDV.Web.Models.Install
{
    public class InstallViewModel
    {
        public List<ApplicationLanguage> Languages { get; set; }

        public AppSettingsJsonDto AppSettingsJson { get; set; }
    }
}
