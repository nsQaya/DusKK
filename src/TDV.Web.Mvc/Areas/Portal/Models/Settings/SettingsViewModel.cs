using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using TDV.Configuration.Tenants.Dto;

namespace TDV.Web.Areas.Portal.Models.Settings
{
    public class SettingsViewModel
    {
        public TenantSettingsEditDto Settings { get; set; }
        
        public List<ComboboxItemDto> TimezoneItems { get; set; }
        
        public List<string> EnabledSocialLoginSettings { get; set; } = new List<string>();

        public List<string> GetOpenIdConnectResponseTypes()
        {
            return (Settings.ExternalLoginProviderSettings.OpenIdConnect.ResponseType??"").Split(',')
                .Select(x => x.Trim()).ToList();
        }
    }
}