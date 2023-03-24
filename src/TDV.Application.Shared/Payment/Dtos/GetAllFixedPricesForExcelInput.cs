using Abp.Application.Services.Dto;
using System;

namespace TDV.Payment.Dtos
{
    public class GetAllFixedPricesForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}