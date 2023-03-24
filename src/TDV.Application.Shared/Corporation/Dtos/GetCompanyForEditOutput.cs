using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Corporation.Dtos
{
    public class GetCompanyForEditOutput
    {
        public CreateOrEditCompanyDto Company { get; set; }

        public string OrganizationUnitDisplayName { get; set; }

        public string CityDisplayProperty { get; set; }

        public string QuarterName { get; set; }

        public int CountryId { get; set; }

        public int CityId { get; set; }

        public int DistrictId { get; set; }

        public int QuarterId { get; set; }
    }
}