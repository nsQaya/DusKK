using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TDV.Flight.Dtos
{
    public class CreateOrEditAirportDto : EntityDto<int?>
    {

        [Required]
        [StringLength(AirportConsts.MaxCodeLength, MinimumLength = AirportConsts.MinCodeLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(AirportConsts.MaxNameLength, MinimumLength = AirportConsts.MinNameLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public int CountryId { get; set; }

        public int? CityId { get; set; }
        public List<AirportRegionDto> Regions { get; set; }

    }
}