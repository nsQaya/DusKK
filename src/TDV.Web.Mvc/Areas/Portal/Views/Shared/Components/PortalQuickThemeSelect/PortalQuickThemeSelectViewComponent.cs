using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Components.
    PortalQuickThemeSelect
{
    public class PortalQuickThemeSelectViewComponent : TDVViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, string iconClass = "flaticon-interface-7 fs-2")
        {
            return Task.FromResult<IViewComponentResult>(View(new QuickThemeSelectionViewModel
            {
                CssClass = cssClass,
                IconClass = iconClass
            }));
        }
    }
}
