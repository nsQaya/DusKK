using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class CreateOrEditFixedPriceDto : EntityDto<int?>
    {

        [StringLength(FixedPriceConsts.MaxNameLength, MinimumLength = FixedPriceConsts.MinNameLength)]
        public string Name { get; set; }

        [StringLength(FixedPriceConsts.MaxDescriptionLength, MinimumLength = FixedPriceConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}