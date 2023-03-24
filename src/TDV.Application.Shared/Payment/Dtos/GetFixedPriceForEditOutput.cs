using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class GetFixedPriceForEditOutput
    {
        public CreateOrEditFixedPriceDto FixedPrice { get; set; }

    }
}