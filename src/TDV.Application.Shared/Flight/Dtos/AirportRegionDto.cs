using System;
using Abp.Application.Services.Dto;

namespace TDV.Flight.Dtos
{
    public class AirportRegionDto : EntityDto
    {

        public int AirportId { get; set; }

        public int RegionId { get; set; }

    }
}