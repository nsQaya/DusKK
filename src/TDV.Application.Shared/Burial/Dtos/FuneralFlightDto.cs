using System;
using Abp.Application.Services.Dto;

namespace TDV.Burial.Dtos
{
    public class FuneralFlightDto : EntityDto
    {
        public string No { get; set; }

        public string Code { get; set; }

        public DateTime LiftOffDate { get; set; }

        public DateTime LandingDate { get; set; }

        public int? FuneralId { get; set; }

        public int AirlineCompanyId { get; set; }

        public int LiftOffAirportId { get; set; }

        public int LangingAirportId { get; set; }

    }
}