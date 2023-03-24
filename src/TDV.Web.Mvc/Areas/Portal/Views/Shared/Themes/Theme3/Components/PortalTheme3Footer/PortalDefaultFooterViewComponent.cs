﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Session;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Themes.Theme3.Components.PortalTheme3Footer
{
    public class PortalTheme3FooterViewComponent : TDVViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public PortalTheme3FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
