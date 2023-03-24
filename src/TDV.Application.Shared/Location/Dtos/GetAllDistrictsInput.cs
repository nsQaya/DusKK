using Abp.Application.Services.Dto;
using System;

namespace TDV.Location.Dtos
{
    public class GetAllDistrictsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public int? MaxOrderFilter { get; set; }
        public int? MinOrderFilter { get; set; }

        public int? IsActiveFilter { get; set; }

        public string CityDisplayPropertyFilter { get; set; }

        public string RegionNameFilter { get; set; }

    }
}