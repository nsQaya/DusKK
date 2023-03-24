using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Session;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Themes.Default.Components.PortalDefaultBrand
{
    public class PortalDefaultBrandViewComponent : TDVViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public PortalDefaultBrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
