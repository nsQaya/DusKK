using Abp.Application.Services.Dto;
using System;

namespace TDV.Flight.Dtos
{
    public class GetAllAirportRegionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string AirportDisplayPropertyFilter { get; set; }

        public string RegionNameFilter { get; set; }

    }
}