using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class CreateOrEditFuneralTypeDto : EntityDto<int?>
    {

        [Required]
        [StringLength(FuneralTypeConsts.MaxDescriptionLength, MinimumLength = FuneralTypeConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public bool IsDefault { get; set; }

    }
}