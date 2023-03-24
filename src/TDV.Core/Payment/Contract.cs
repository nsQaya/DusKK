using TDV.Payment;
using TDV.Location;
using TDV.Corporation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Payment
{
    [Table("Contracts")]
    public class Contract : FullAuditedEntity
    {

        public virtual string Formule { get; set; }

        public virtual DateTime StartDate { get; set; }

        public virtual DateTime EndDate { get; set; }

        public virtual CurrencyType CurrencyType { get; set; }

        public virtual int RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region RegionFk { get; set; }

        public virtual int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company CompanyFk { get; set; }

    }
}