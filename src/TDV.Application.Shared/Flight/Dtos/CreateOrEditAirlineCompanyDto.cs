using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Flight.Dtos
{
    public class CreateOrEditAirlineCompanyDto : EntityDto<int?>
    {

        [Required]
        [StringLength(AirlineCompanyConsts.MaxCodeLength, MinimumLength = AirlineCompanyConsts.MinCodeLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(AirlineCompanyConsts.MaxNameLength, MinimumLength = AirlineCompanyConsts.MinNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AirlineCompanyConsts.MaxLadingPrefixLength, MinimumLength = AirlineCompanyConsts.MinLadingPrefixLength)]
        public string LadingPrefix { get; set; }

        [Required]
        [StringLength(AirlineCompanyConsts.MaxFlightPrefixLength, MinimumLength = AirlineCompanyConsts.MinFlightPrefixLength)]
        public string FlightPrefix { get; set; }

    }
}