using Abp.Application.Services.Dto;
using System;

namespace TDV.Flight.Dtos
{
    public class GetAllAirportsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string CountryDisplayPropertyFilter { get; set; }

        public string CityDisplayPropertyFilter { get; set; }

    }
}