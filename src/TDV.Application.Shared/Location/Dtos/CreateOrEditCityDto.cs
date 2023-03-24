using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class CreateOrEditCityDto : EntityDto<int?>
    {

        [StringLength(CityConsts.MaxCodeLength, MinimumLength = CityConsts.MinCodeLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(CityConsts.MaxNameLength, MinimumLength = CityConsts.MinNameLength)]
        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public int CountryId { get; set; }

    }
}