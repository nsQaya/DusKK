using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Components.PortalToggleDarkMode
{
    public class PortalToggleDarkModeViewComponent : TDVViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, bool isDarkModeActive)
        {
            return Task.FromResult<IViewComponentResult>(View(new ToggleDarkModeViewModel(cssClass, isDarkModeActive)));
        }
    }
}