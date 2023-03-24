using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class CreateOrEditCompanyTransactionDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CompanyTransactionConsts.MaxInOutLength, MinimumLength = CompanyTransactionConsts.MinInOutLength)]
        public string InOut { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(CompanyTransactionConsts.MaxNoLength, MinimumLength = CompanyTransactionConsts.MinNoLength)]
        public string No { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public decimal Price { get; set; }

        public int TaxRate { get; set; }

        public decimal Total { get; set; }

        public bool IsTransferred { get; set; }

        public int CompanyId { get; set; }

        public int FuneralId { get; set; }

        public int Type { get; set; }

        public int CurrencyId { get; set; }

        public int UnitType { get; set; }

    }
}