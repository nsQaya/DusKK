using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class GetContractFormuleForEditOutput
    {
        public CreateOrEditContractFormuleDto ContractFormule { get; set; }

    }
}