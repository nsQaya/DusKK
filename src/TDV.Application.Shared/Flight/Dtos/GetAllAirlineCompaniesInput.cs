using Abp.Application.Services.Dto;
using System;

namespace TDV.Flight.Dtos
{
    public class GetAllAirlineCompaniesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string LadingPrefixFilter { get; set; }

        public string FlightPrefixFilter { get; set; }

    }
}