using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class CreateOrEditContractFormuleDto : EntityDto<int?>
    {

        [Required]
        public string Formule { get; set; }

        [StringLength(ContractFormuleConsts.MaxDescriptionLength, MinimumLength = ContractFormuleConsts.MinDescriptionLength)]
        public string Description { get; set; }

    }
}