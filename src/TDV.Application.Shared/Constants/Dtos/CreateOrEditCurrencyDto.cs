using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using TDV.Constants;

namespace TDV.Constants.Dtos
{
    public class CreateOrEditCurrencyDto : EntityDto<int?>
    {

        [Required]
        [StringLength(CurrencyConsts.MaxCodeLength, MinimumLength = CurrencyConsts.MinCodeLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(CurrencyConsts.MaxSymbolLength, MinimumLength = CurrencyConsts.MinSymbolLength)]
        public string Symbol { get; set; }

        public int OrderNumber { get; set; }

        public int NumberCode { get; set; }

        public bool IsActive { get; set; }

    }
}