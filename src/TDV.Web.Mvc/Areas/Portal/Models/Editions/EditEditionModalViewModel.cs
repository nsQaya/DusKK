using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TDV.Editions.Dto;
using TDV.Web.Areas.Portal.Models.Common;

namespace TDV.Web.Areas.Portal.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionEditOutput))]
    public class EditEditionModalViewModel : GetEditionEditOutput, IFeatureEditViewModel
    {
        public bool IsEditMode => Edition.Id.HasValue;

        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

        public IReadOnlyList<ComboboxItemDto> FreeEditionItems { get; set; }
    }
}