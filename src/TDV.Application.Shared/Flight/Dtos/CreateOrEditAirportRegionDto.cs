using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Flight.Dtos
{
    public class CreateOrEditAirportRegionDto : EntityDto<int?>
    {

        public int AirportId { get; set; }

        public int RegionId { get; set; }

    }
}