using Abp.Application.Navigation;

namespace TDV.Web.Areas.Portal.Views.Shared.Components.PortalMenu
{
    public class UserMenuItemViewModel
    {
        public UserMenuItem MenuItem { get; set; }

        public string CurrentPageName { get; set; }

        public int MenuItemIndex { get; set; }

        public int ItemDepth { get; set; }

        public bool RootLevel { get; set; }
        
        public bool IsTabMenuUsed { get; set; }
        
        public bool IconMenu { get; set; }
    }
}
