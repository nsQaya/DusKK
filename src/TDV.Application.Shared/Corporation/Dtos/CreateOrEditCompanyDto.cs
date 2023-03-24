using TDV.Corporation;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Corporation.Dtos
{
    public class CreateOrEditCompanyDto : EntityDto<int?>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public CompanyType Type { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxTaxAdministrationLength, MinimumLength = CompanyConsts.MinTaxAdministrationLength)]
        public string TaxAdministration { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxTaxNoLength, MinimumLength = CompanyConsts.MinTaxNoLength)]
        public string TaxNo { get; set; }

        public string Website { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxPhoneLength, MinimumLength = CompanyConsts.MinPhoneLength)]
        public string Phone { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxFaxLength, MinimumLength = CompanyConsts.MinFaxLength)]
        public string Fax { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxEmailLength, MinimumLength = CompanyConsts.MinEmailLength)]
        public string Email { get; set; }

        [Required]
        public string Address { get; set; }

        public string RunningCode { get; set; }

        public string Prefix { get; set; }

        public long? OrganizationUnitId { get; set; }

        public int CityId { get; set; }

        public int QuarterId { get; set; }

    }
}