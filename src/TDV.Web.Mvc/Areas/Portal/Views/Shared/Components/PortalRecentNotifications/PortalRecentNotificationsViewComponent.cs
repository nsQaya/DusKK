using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Components.PortalRecentNotifications
{
    public class PortalRecentNotificationsViewComponent : TDVViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, string iconClass = "flaticon-alert-2 unread-notification fs-2")
        {
            var model = new RecentNotificationsViewModel
            {
                CssClass = cssClass,
                IconClass = iconClass
            };
            
            return Task.FromResult<IViewComponentResult>(View(model));
        }
    }
}
