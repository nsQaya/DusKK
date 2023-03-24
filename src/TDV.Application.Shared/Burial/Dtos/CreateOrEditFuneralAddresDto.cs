using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Burial.Dtos
{
    public class CreateOrEditFuneralAddresDto : EntityDto<int?>
    {

        [StringLength(FuneralAddresConsts.MaxDescriptionLength, MinimumLength = FuneralAddresConsts.MinDescriptionLength)]
        public string Description { get; set; }

        [Required]
        public string Address { get; set; }

        public int FuneralId { get; set; }

        public int QuarterId { get; set; }

    }
}