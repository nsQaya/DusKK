using Abp.Application.Services.Dto;
using System;

namespace TDV.Payment.Dtos
{
    public class GetAllCompanyTransactionsForExcelInput
    {
        public string Filter { get; set; }

        public string InOutFilter { get; set; }

        public DateTime? MaxDateFilter { get; set; }
        public DateTime? MinDateFilter { get; set; }

        public string NoFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public decimal? MaxAmountFilter { get; set; }
        public decimal? MinAmountFilter { get; set; }

        public decimal? MaxPriceFilter { get; set; }
        public decimal? MinPriceFilter { get; set; }

        public int? MaxTaxRateFilter { get; set; }
        public int? MinTaxRateFilter { get; set; }

        public decimal? MaxTotalFilter { get; set; }
        public decimal? MinTotalFilter { get; set; }

        public int? IsTransferredFilter { get; set; }

        public string CompanyTaxAdministrationFilter { get; set; }

        public string FuneralDisplayPropertyFilter { get; set; }

        public string DataListValueFilter { get; set; }

        public string CurrencyCodeFilter { get; set; }

        public string DataListValue2Filter { get; set; }

    }
}