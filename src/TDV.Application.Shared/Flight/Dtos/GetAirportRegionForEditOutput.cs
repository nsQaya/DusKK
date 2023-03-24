using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Flight.Dtos
{
    public class GetAirportRegionForEditOutput
    {
        public CreateOrEditAirportRegionDto AirportRegion { get; set; }

        public string AirportDisplayProperty { get; set; }

        public string RegionName { get; set; }

    }
}