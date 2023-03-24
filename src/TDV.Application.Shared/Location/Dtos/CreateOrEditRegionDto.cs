using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Location.Dtos
{
    public class CreateOrEditRegionDto : EntityDto<int?>
    {

        [Required]
        [StringLength(RegionConsts.MaxNameLength, MinimumLength = RegionConsts.MinNameLength)]
        public string Name { get; set; }

        public int Order { get; set; }

        public bool IsActive { get; set; }

        public int FixedPriceId { get; set; }

    }
}