using Abp.Application.Services.Dto;
using System;

namespace TDV.Payment.Dtos
{
    public class GetAllContractFormulesForExcelInput
    {
        public string Filter { get; set; }

        public string FormuleFilter { get; set; }

    }
}