using Abp.AspNetCore.Mvc.Views;

namespace TDV.Web.Views
{
    public abstract class TDVRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected TDVRazorPage()
        {
            LocalizationSourceName = TDVConsts.LocalizationSourceName;
        }
    }
}
