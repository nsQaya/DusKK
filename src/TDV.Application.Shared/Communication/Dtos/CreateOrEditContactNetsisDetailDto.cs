using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Communication.Dtos
{
    public class CreateOrEditContactNetsisDetailDto : EntityDto<int?>
    {

        [Required]
        [StringLength(ContactNetsisDetailConsts.MaxNetsisNoLength, MinimumLength = ContactNetsisDetailConsts.MinNetsisNoLength)]
        public string NetsisNo { get; set; }

        [Required]
        [StringLength(ContactNetsisDetailConsts.MaxRegistryNoLength, MinimumLength = ContactNetsisDetailConsts.MinRegistryNoLength)]
        public string RegistryNo { get; set; }

        public int ContactId { get; set; }

    }
}