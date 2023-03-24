using TDV.Corporation;
using TDV.Burial;
using TDV.Constants;
using TDV.Constants;
using TDV.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Payment
{
    [Table("CompanyTransactions")]
    public class CompanyTransaction : FullAuditedEntity
    {

        [Required]
        [StringLength(CompanyTransactionConsts.MaxInOutLength, MinimumLength = CompanyTransactionConsts.MinInOutLength)]
        public virtual string InOut { get; set; }

        public virtual DateTime Date { get; set; }

        [Required]
        [StringLength(CompanyTransactionConsts.MaxNoLength, MinimumLength = CompanyTransactionConsts.MinNoLength)]
        public virtual string No { get; set; }

        public virtual string Description { get; set; }

        public virtual decimal Amount { get; set; }

        public virtual decimal Price { get; set; }

        public virtual int TaxRate { get; set; }

        public virtual decimal Total { get; set; }

        public virtual bool IsTransferred { get; set; }

        public virtual int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company CompanyFk { get; set; }

        public virtual int FuneralId { get; set; }

        [ForeignKey("FuneralId")]
        public Funeral FuneralFk { get; set; }

        public virtual int Type { get; set; }

        [ForeignKey("Type")]
        public DataList TypeFk { get; set; }

        public virtual int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency CurrencyFk { get; set; }

        public virtual int UnitType { get; set; }

        [ForeignKey("UnitType")]
        public DataList UnitTypeFk { get; set; }

    }
}