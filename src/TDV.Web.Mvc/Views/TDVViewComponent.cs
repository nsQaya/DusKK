using Abp.AspNetCore.Mvc.ViewComponents;

namespace TDV.Web.Views
{
    public abstract class TDVViewComponent : AbpViewComponent
    {
        protected TDVViewComponent()
        {
            LocalizationSourceName = TDVConsts.LocalizationSourceName;
        }
    }
}