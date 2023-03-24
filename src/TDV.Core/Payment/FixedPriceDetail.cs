using TDV.Payment;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Payment
{
    [Table("FixedPriceDetails")]
    public class FixedPriceDetail : FullAuditedEntity
    {

        public virtual PaymentMethodType Type { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }

        public virtual decimal Price { get; set; }

        public virtual int? FixedPriceId { get; set; }

        [ForeignKey("FixedPriceId")]
        public FixedPrice FixedPriceFk { get; set; }

    }
}