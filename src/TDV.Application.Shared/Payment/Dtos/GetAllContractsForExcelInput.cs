using Abp.Application.Services.Dto;
using System;

namespace TDV.Payment.Dtos
{
    public class GetAllContractsForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public int? CurrencyTypeFilter { get; set; }

        public string RegionNameFilter { get; set; }

        public string CompanyDisplayPropertyFilter { get; set; }

    }
}