using TDV.Location.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Regions
{
    public class CreateOrEditRegionModalViewModel
    {
        public CreateOrEditRegionDto Region { get; set; }

        public string FixedPriceName { get; set; }

        public List<FixedPriceLookupTableDto> RegionFixedPriceList { get; set; }

        public bool IsEditMode => Region.Id.HasValue;
    }
}