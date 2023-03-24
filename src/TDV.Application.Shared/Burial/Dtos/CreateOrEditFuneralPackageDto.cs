using TDV.Burial;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class CreateOrEditFuneralPackageDto : EntityDto<int?>
    {

        public FuneralStatus Status { get; set; }

        [Required]
        [StringLength(FuneralPackageConsts.MaxCodeLength, MinimumLength = FuneralPackageConsts.MinCodeLength)]
        public string Code { get; set; }

        [StringLength(FuneralPackageConsts.MaxDescriptionLength, MinimumLength = FuneralPackageConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}