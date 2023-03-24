using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace TDV.Web.Public.Views
{
    public abstract class TDVRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected TDVRazorPage()
        {
            LocalizationSourceName = TDVConsts.LocalizationSourceName;
        }
    }
}
