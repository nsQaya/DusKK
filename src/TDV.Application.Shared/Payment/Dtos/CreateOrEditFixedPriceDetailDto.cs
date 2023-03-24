using TDV.Payment;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Payment.Dtos
{
    public class CreateOrEditFixedPriceDetailDto : EntityDto<int?>
    {

        public PaymentMethodType Type { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public CurrencyType CurrencyType { get; set; }

        public decimal Price { get; set; }

        public int? FixedPriceId { get; set; }

    }
}