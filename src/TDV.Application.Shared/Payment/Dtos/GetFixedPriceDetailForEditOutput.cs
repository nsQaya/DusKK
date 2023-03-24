using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class GetFixedPriceDetailForEditOutput
    {
        public CreateOrEditFixedPriceDetailDto FixedPriceDetail { get; set; }

        public string FixedPriceName { get; set; }

    }
}