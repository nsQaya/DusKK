using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class CreateOrEditDistrictDto : EntityDto<int?>
    {

        [Required]
        [StringLength(DistrictConsts.MaxNameLength, MinimumLength = DistrictConsts.MinNameLength)]
        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }
        public int CountryId { get; set; }

        public int CityId { get; set; }

        public int RegionId { get; set; }

    }
}