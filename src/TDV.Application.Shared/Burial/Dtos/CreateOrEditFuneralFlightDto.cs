using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class CreateOrEditFuneralFlightDto : EntityDto<int?>
    {

        [StringLength(FuneralFlightConsts.MaxNoLength, MinimumLength = FuneralFlightConsts.MinNoLength)]
        public string No { get; set; }

        [Required]
        [StringLength(FuneralFlightConsts.MaxCodeLength, MinimumLength = FuneralFlightConsts.MinCodeLength)]
        public string Code { get; set; }

        public DateTime LiftOffDate { get; set; }

        public DateTime LandingDate { get; set; }

        public int? FuneralId { get; set; }

        public int AirlineCompanyId { get; set; }

        public int LiftOffAirportId { get; set; }

        public int LangingAirportId { get; set; }

    }
}