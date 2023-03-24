using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Flight
{
    [Table("AirlineCompanies")]
    public class AirlineCompany : FullAuditedEntity
    {

        [Required]
        [StringLength(AirlineCompanyConsts.MaxCodeLength, MinimumLength = AirlineCompanyConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(AirlineCompanyConsts.MaxNameLength, MinimumLength = AirlineCompanyConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(AirlineCompanyConsts.MaxLadingPrefixLength, MinimumLength = AirlineCompanyConsts.MinLadingPrefixLength)]
        public virtual string LadingPrefix { get; set; }

        [Required]
        [StringLength(AirlineCompanyConsts.MaxFlightPrefixLength, MinimumLength = AirlineCompanyConsts.MinFlightPrefixLength)]
        public virtual string FlightPrefix { get; set; }

    }
}