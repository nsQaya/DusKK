using Abp.Application.Services.Dto;
using System;

namespace TDV.Corporation.Dtos
{
    public class GetAllCompanyContactsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string TitleFilter { get; set; }

        public string CompanyDisplayPropertyFilter { get; set; }

        public string ContactNameFilter { get; set; }

        public int OrganizationUnitId { get; set; }
    }
}