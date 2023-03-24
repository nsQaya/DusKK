using TDV.Burial;
using TDV.Flight;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Burial
{
    [Table("FuneralFlights")]
    public class FuneralFlight : FullAuditedEntity
    {

        [StringLength(FuneralFlightConsts.MaxNoLength, MinimumLength = FuneralFlightConsts.MinNoLength)]
        public virtual string No { get; set; }

        [Required]
        [StringLength(FuneralFlightConsts.MaxCodeLength, MinimumLength = FuneralFlightConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        public virtual DateTime LiftOffDate { get; set; }

        public virtual DateTime LandingDate { get; set; }

        public virtual int? FuneralId { get; set; }

        [ForeignKey("FuneralId")]
        public Funeral FuneralFk { get; set; }

        public virtual int AirlineCompanyId { get; set; }

        [ForeignKey("AirlineCompanyId")]
        public AirlineCompany AirlineCompanyFk { get; set; }

        public virtual int LiftOffAirportId { get; set; }

        [ForeignKey("LiftOffAirportId")]
        public Airport LiftOffAirportFk { get; set; }

        public virtual int LangingAirportId { get; set; }

        [ForeignKey("LangingAirportId")]
        public Airport LangingAirportFk { get; set; }

    }
}