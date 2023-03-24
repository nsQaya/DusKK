using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class CreateOrEditCountryDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CountryConsts.MaxCodeLength, MinimumLength = CountryConsts.MinCodeLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxNameLength, MinimumLength = CountryConsts.MinNameLength)]
        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

    }
}