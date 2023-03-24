using TDV.Corporation.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Companies
{
    public class CreateOrEditCompanyModalViewModel
    {
        public CreateOrEditCompanyDto Company { get; set; }

        public string OrganizationUnitDisplayName { get; set; }

        public string CityDisplayProperty { get; set; }

        public string QuarterName { get; set; }

        public List<CompanyCityLookupTableDto> CompanyCityList { get; set; }

        public List<CompanyQuarterLookupTableDto> CompanyQuarterList { get; set; }

        public bool IsEditMode => Company.Id.HasValue;
    }
}