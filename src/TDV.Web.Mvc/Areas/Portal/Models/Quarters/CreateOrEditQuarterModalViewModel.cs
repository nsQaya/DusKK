using TDV.Location.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Quarters
{
    public class CreateOrEditQuarterModalViewModel
    {
        public CreateOrEditQuarterDto Quarter { get; set; }

        public string DistrictName { get; set; }

        public List<CountryLookupTableDto> CountryList { get; set; }
        public List<CityLookupTableDto> CityList { get; set; }
        public List<DistrictLookupTableDto> QuarterDistrictList { get; set; }

        public bool IsEditMode => Quarter.Id.HasValue;
    }
}