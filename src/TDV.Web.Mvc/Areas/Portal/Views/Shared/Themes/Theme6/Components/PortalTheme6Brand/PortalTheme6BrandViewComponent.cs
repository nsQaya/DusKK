using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Session;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Themes.Theme6.Components.PortalTheme6Brand
{
    public class PortalTheme6BrandViewComponent : TDVViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public PortalTheme6BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string skin = "dark-sm")
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync(),
            };

            ViewBag.BrandLogoSkin = skin;

            return View(headerModel);
        }
    }
}
