using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Extensions;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Razor.TagHelpers;
using TDV.UiCustomization;
using TDV.UiCustomization.Dto;

namespace TDV.Web.TagHelpers.Customs
{
    public class StepperNavigationItem
    {
        public int Step { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public StepperNavigationItem(int step, string text, string description)
        {
            Step = step;
            Text = text;
            Description = description;
        }
    }

    [HtmlTargetElement("stepper-navs")]
    public class TDVStepperNavigationTagHelper : TagHelper
    {

        public List<StepperNavigationItem> Navs { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var template = GetTemplate();
            output.TagName = "div";
            output.Attributes.Add("class", "stepper-nav flex-center flex-wrap mb-10");
            output.Content.AppendHtml(template);
        }

        private string GetTemplate()
        {
            return Navs.Select(x => GetStepNavigation(x)).JoinAsString("");
        }

        private string GetStepNavigation(StepperNavigationItem Item)
        {
            return $@"
                <!--begin::Step-->
                <div class='stepper-item mx-8 my-4 {(Navs.First() == Item ? "current" : "")}' data-kt-stepper-element='nav'>
                    <!--begin::Wrapper-->
                    <div class='stepper-wrapper d-flex align-items-center'>
                        <!--begin::Icon-->
                        <div class='stepper-icon w-40px h-40px'>
                            <i class='stepper-check fas fa-check'></i>
                            <span class='stepper-number'>{Item.Step}</span>
                        </div>
                        <!--end::Icon-->

                        <!--begin::Label-->
                        <div class='stepper-label'>
                            <h3 class='stepper-title'>{Item.Text}</h3>
                            <div class='stepper-desc'>{Item.Description}</div>
                        </div>
                        <!--end::Label-->
                    </div>
                    <!--end::Wrapper-->

                    <!--begin::Line-->
                    <div class='stepper-line h-40px'></div>
                    <!--end::Line-->
                </div>
                <!--end::Step-->
            ";
        }
    }
}