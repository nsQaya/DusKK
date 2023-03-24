using TDV.Flight.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;
using TDV.Location.Dtos;

namespace TDV.Web.Areas.Portal.Models.Airports
{
    public class CreateOrEditAirportModalViewModel
    {
        public CreateOrEditAirportDto Airport { get; set; }

        public string CountryDisplayProperty { get; set; }

        public string CityDisplayProperty { get; set; }

        public List<CountryLookupTableDto> AirportCountryList { get; set; }

        public List<CityLookupTableDto> AirportCityList { get; set; }

        public bool IsEditMode => Airport.Id.HasValue;
    }
}