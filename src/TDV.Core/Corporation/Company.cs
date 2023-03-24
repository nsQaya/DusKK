using TDV.Corporation;
using Abp.Organizations;
using TDV.Location;
using TDV.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;
using TDV.Payment;

namespace TDV.Corporation
{
    [Table("Companies")]
    public class Company : FullAuditedEntity
    {

        public virtual bool IsActive { get; set; }

        public virtual CompanyType Type { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxTaxAdministrationLength, MinimumLength = CompanyConsts.MinTaxAdministrationLength)]
        public virtual string TaxAdministration { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxTaxNoLength, MinimumLength = CompanyConsts.MinTaxNoLength)]
        public virtual string TaxNo { get; set; }

        public virtual string Website { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxPhoneLength, MinimumLength = CompanyConsts.MinPhoneLength)]
        public virtual string Phone { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxFaxLength, MinimumLength = CompanyConsts.MinFaxLength)]
        public virtual string Fax { get; set; }

        [Required]
        [StringLength(CompanyConsts.MaxEmailLength, MinimumLength = CompanyConsts.MinEmailLength)]
        public virtual string Email { get; set; }

        [Required]
        public virtual string Address { get; set; }

        public virtual string RunningCode { get; set; }

        public virtual string Prefix { get; set; }

        public virtual long OrganizationUnitId { get; set; }

        [ForeignKey("OrganizationUnitId")]
        public OrganizationUnit OrganizationUnitFk { get; set; }

        public virtual int CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual int QuarterId { get; set; }

        [ForeignKey("QuarterId")]
        public Quarter QuarterFk { get; set; }

        public List<Vehicle> Vehicles { get; set; }
        public List<Contract> Contracts { get; set; }
    }
}