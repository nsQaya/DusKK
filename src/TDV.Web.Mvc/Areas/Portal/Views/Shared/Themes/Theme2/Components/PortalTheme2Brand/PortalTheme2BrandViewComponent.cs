﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Session;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Themes.Theme2.Components.PortalTheme2Brand
{
    public class PortalTheme2BrandViewComponent : TDVViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public PortalTheme2BrandViewComponent(IPerRequestSessionCache sessionCache)
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
