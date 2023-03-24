using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Layout;
using TDV.Web.Views;

namespace TDV.Web.Areas.Portal.Views.Shared.Components.PortalChatToggler
{
    public class PortalChatTogglerViewComponent : TDVViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass, string iconClass = "flaticon-chat-2 fs-2")
        {
            return Task.FromResult<IViewComponentResult>(View(new ChatTogglerViewModel
            {
                CssClass = cssClass,
                IconClass = iconClass
            }));
        }
    }
}
