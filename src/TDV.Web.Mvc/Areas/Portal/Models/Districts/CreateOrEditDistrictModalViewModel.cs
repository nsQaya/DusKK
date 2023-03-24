using TDV.Location.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Districts
{
    public class CreateOrEditDistrictModalViewModel
    {
        public CreateOrEditDistrictDto District { get; set; }

        public string CityDisplayProperty { get; set; }

        public string RegionName { get; set; }

        public List<CountryLookupTableDto> CountryList { get; set; }
        public List<CityLookupTableDto> DistrictCityList { get; set; }

        public List<RegionLookupTableDto> DistrictRegionList { get; set; }

        public bool IsEditMode => District.Id.HasValue;
    }
}