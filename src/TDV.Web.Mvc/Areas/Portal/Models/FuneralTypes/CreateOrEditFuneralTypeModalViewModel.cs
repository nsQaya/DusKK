using TDV.Burial.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FuneralTypes
{
    public class CreateOrEditFuneralTypeModalViewModel
    {
        public CreateOrEditFuneralTypeDto FuneralType { get; set; }

        public bool IsEditMode => FuneralType.Id.HasValue;
    }
}