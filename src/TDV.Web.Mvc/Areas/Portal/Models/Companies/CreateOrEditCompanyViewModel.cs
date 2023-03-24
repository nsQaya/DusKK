using TDV.Corporation.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;
using TDV.Location.Dtos;

namespace TDV.Web.Areas.Portal.Models.Companies
{
    public class CreateOrEditCompanyViewModel
    {
        public CreateOrEditCompanyDto Company { get; set; }

        public string OrganizationUnitDisplayName { get; set; }

        public string CityDisplayProperty { get; set; }

        public string QuarterName { get; set; }
        public List<CountryLookupTableDto> CountryList { get; set; }

        public List<CityLookupTableDto> CityList { get; set; }

        public List<DistrictLookupTableDto> DistrictList { get; set; }

        public List<QuarterLookupTableDto> QuarterList { get; set; }

        public int CountryId { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public int QuarterId { get; set; }

        public bool IsEditMode => Company.Id.HasValue;
    }
}