using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Extensions;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TDV.UiCustomization;
using TDV.UiCustomization.Dto;

namespace TDV.Web.TagHelpers.Customs
{


    [HtmlTargetElement("custom-tab")]
    public class TDVCustomTabTagHelper : TagHelper
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var tabs = (List<TabItem>)context.Items["tabs"];

            var tabBuilder = new TagBuilder("div");
            tabBuilder.Attributes.Add("id", "{erencan.kurt}");
            tabBuilder.AddCssClass("tab-pane fade");
            tabBuilder.Attributes.Add("role", "tabpanel");

            if (IsActive)
            {
                tabBuilder.AddCssClass("show active");
            }
            else
            {
                tabBuilder.AddCssClass("{erencan.kurt.active}");
            }

            tabBuilder.InnerHtml.AppendHtml(await output.GetChildContentAsync());

            string content = null;
            using(var writer= new StringWriter())
            {
                tabBuilder.WriteTo(writer,HtmlEncoder.Default);
                content= writer.ToString();
            }

            tabs.Add(new()
            {
                Name = Name,
                Icon = Icon,
                Content = content
            });

            output.SuppressOutput();
        }
    }
}