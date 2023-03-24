using TDV.Flight;
using TDV.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Flight
{
    [Table("AirportRegions")]
    public class AirportRegion : FullAuditedEntity
    {

        public virtual int AirportId { get; set; }

        [ForeignKey("AirportId")]
        public Airport AirportFk { get; set; }

        public virtual int RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region RegionFk { get; set; }

    }
}