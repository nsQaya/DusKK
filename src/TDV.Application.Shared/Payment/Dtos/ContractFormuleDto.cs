using System;
using Abp.Application.Services.Dto;

namespace TDV.Payment.Dtos
{
    public class ContractFormuleDto : EntityDto
    {
        public string Formule { get; set; }

        public string Description { get; set; }

    }
}