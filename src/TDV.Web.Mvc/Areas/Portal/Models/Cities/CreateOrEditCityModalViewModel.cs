using TDV.Location.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Cities
{
    public class CreateOrEditCityModalViewModel
    {
        public CreateOrEditCityDto City { get; set; }

        public string CountryDisplayProperty { get; set; }

        public List<CountryLookupTableDto> CityCountryList { get; set; }

        public bool IsEditMode => City.Id.HasValue;
    }
}