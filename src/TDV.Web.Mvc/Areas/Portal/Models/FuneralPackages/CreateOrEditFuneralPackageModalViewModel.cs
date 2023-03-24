using TDV.Burial.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FuneralPackages
{
    public class CreateOrEditFuneralPackageModalViewModel
    {
        public CreateOrEditFuneralPackageDto FuneralPackage { get; set; }

        public bool IsEditMode => FuneralPackage.Id.HasValue;
    }
}