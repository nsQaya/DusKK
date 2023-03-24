using Abp.Application.Services.Dto;
using System;

namespace TDV.Payment.Dtos
{
    public class GetAllFixedPriceDetailsForExcelInput
    {
        public string Filter { get; set; }

        public int? TypeFilter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public int? CurrencyTypeFilter { get; set; }

        public decimal? MaxPriceFilter { get; set; }
        public decimal? MinPriceFilter { get; set; }

        public string FixedPriceNameFilter { get; set; }

        public int? FixedPriceIdFilter { get; set; }
    }
}