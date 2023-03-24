using TDV.Location;
using TDV.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace TDV.Flight
{
    [Table("Airports")]
    public class Airport : FullAuditedEntity
    {

        [Required]
        [StringLength(AirportConsts.MaxCodeLength, MinimumLength = AirportConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(AirportConsts.MaxNameLength, MinimumLength = AirportConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual int Order { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }

        public virtual int? CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public List<AirportRegion> Regions { get; set; }
    }
}