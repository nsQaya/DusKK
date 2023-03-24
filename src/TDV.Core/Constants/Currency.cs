using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Constants
{
    [Table("Currencies")]
    public class Currency : Entity
    {

        [Required]
        [StringLength(CurrencyConsts.MaxCodeLength, MinimumLength = CurrencyConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(CurrencyConsts.MaxSymbolLength, MinimumLength = CurrencyConsts.MinSymbolLength)]
        public virtual string Symbol { get; set; }

        public virtual int OrderNumber { get; set; }

        public virtual int NumberCode { get; set; }

        public virtual bool IsActive { get; set; }

    }
}