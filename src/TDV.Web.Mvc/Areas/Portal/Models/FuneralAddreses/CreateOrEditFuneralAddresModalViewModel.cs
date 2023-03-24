using TDV.Burial.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FuneralAddreses
{
    public class CreateOrEditFuneralAddresModalViewModel
    {
        public CreateOrEditFuneralAddresDto FuneralAddres { get; set; }

        public string FuneralDisplayProperty { get; set; }

        public string QuarterName { get; set; }

        public bool IsEditMode => FuneralAddres.Id.HasValue;
    }
}