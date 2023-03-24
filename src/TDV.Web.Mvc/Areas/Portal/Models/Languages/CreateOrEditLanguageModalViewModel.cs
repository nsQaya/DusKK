using Abp.AutoMapper;
using TDV.Localization.Dto;

namespace TDV.Web.Areas.Portal.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}