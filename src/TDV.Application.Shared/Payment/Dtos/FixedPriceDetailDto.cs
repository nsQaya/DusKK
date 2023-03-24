using TDV.Payment;

using System;
using Abp.Application.Services.Dto;

namespace TDV.Payment.Dtos
{
    public class FixedPriceDetailDto : EntityDto
    {
        public PaymentMethodType Type { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public CurrencyType CurrencyType { get; set; }

        public decimal Price { get; set; }

        public int? FixedPriceId { get; set; }

    }
}