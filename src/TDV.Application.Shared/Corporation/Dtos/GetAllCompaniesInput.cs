using Abp.Application.Services.Dto;
using System;

namespace TDV.Corporation.Dtos
{
    public class GetAllCompaniesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public int? IsActiveFilter { get; set; }

        public int? TypeFilter { get; set; }

        public string TaxAdministrationFilter { get; set; }

        public string TaxNoFilter { get; set; }

        public string WebsiteFilter { get; set; }

        public string PhoneFilter { get; set; }

        public string FaxFilter { get; set; }

        public string EmailFilter { get; set; }

        public string AddressFilter { get; set; }

        public string RunningCodeFilter { get; set; }

        public string PrefixFilter { get; set; }

        public string OwnerOrganizationUnitDisplayName { get; set; }

        public string CityDisplayPropertyFilter { get; set; }

        public string QuarterNameFilter { get; set; }

    }
}