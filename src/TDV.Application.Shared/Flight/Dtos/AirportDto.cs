using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace TDV.Flight.Dtos
{
    public class AirportDto : EntityDto
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public int CountryId { get; set; }

        public int? CityId { get; set; }
        public List<AirportRegionDto> Regions { get; set; }
    }
}