using System.Linq;
using System.Threading.Tasks;
using Abp.Localization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Components.PortalLanguageSwitch
{
    public class PortalLanguageSwitchViewComponent : TDVViewComponent
    {
        private readonly ILanguageManager _languageManager;

        public PortalLanguageSwitchViewComponent(ILanguageManager languageManager)
        {
            _languageManager = languageManager;
        }

        public Task<IViewComponentResult> InvokeAsync(string cssClass)
        {
            var model = new LanguageSwitchViewModel
            {
                Languages = _languageManager.GetActiveLanguages().ToList(),
                CurrentLanguage = _languageManager.CurrentLanguage,
                CssClass = cssClass
            };
            
            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
