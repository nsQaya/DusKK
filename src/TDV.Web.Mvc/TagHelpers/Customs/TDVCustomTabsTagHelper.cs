using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Extensions;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TDV.UiCustomization;
using TDV.UiCustomization.Dto;

namespace TDV.Web.TagHelpers.Customs
{
    public class TabItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
    }

    [HtmlTargetElement("custom-tabs")]
    public class TDVCustomTabsTagHelper : TagHelper
    {
        private List<TabItem> tabs;
        private string uniqID;
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            uniqID = Guid.NewGuid().ToString("N");

            tabs = new List<TabItem>();
            context.Items["tabs"] = tabs;

            await output.GetChildContentAsync();

            var outputString = new StringBuilder();

            var hasActive = tabs.Find(x => x.IsActive == true) != null;

            tabs.ForEach(tab =>
            {
                tab.Content = tab.Content.Replace("{erencan.kurt}", "tab-"+ uniqID + "-" + tabs.LastIndexOf(tab));

                if (!hasActive && tabs.LastIndexOf(tab)==0)
                {
                    tab.Content = tab.Content.Replace("{erencan.kurt.active}", "show active");
                    tab.IsActive = true;
                }
                else
                {
                    tab.Content = tab.Content.Replace("{erencan.kurt.active}", "");
                }

                outputString.AppendLine(tab.Content);
            });

            output.TagName = "div";
            output.Attributes.Add("class", "custom-tabs");

            var Navs = CreateNavs();
            output.PreContent.AppendHtml(Navs);
            output.PostContent.AppendHtml("<div class='row'><div class='tab-content'>");
            output.PostContent.AppendHtml(outputString.ToString());
            output.PostContent.AppendHtml("</div></div>");
           
        }

        private string CreateNav(TabItem item)
        {
            return $@"
            <li class='nav-item'>
                <a class='nav-link w-100 {((item.IsActive==true)?"active":"")} btn btn-flex btn-active-light-info' data-bs-toggle='tab' href='#tab-{uniqID}-{tabs.LastIndexOf(item)}'>
                    <span class='svg-icon svg-icon-2 flaticon {item.Icon}'></span>
                    <span class='d-flex flex-column align-items-start'>
                        <span class='fs-4 fw-bold'>{item.Name}</span>
                    </span>
                </a>
            </li>";
        }
        private string CreateNavs()
        {
            return $@"
            <ul class='nav nav-tabs nav-line-tabs mb-5 fs-6'>
                {tabs.Select(x => CreateNav(x)).JoinAsString("")}
            </ul>";
        }
    }
}