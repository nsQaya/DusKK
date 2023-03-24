using TDV.Corporation;

using System;
using Abp.Application.Services.Dto;

namespace TDV.Corporation.Dtos
{
    public class CompanyDto : EntityDto
    {
        public bool IsActive { get; set; }

        public CompanyType Type { get; set; }

        public string TaxAdministration { get; set; }

        public string TaxNo { get; set; }

        public string Website { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string RunningCode { get; set; }

        public string Prefix { get; set; }

        public long OrganizationUnitId { get; set; }

        public int CityId { get; set; }

        public int QuarterId { get; set; }

    }
}