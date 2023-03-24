using TDV.Payment;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Location
{
    [Table("Regions")]
    public class Region : FullAuditedEntity
    {

        [Required]
        [StringLength(RegionConsts.MaxNameLength, MinimumLength = RegionConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int FixedPriceId { get; set; }

        [ForeignKey("FixedPriceId")]
        public FixedPrice FixedPriceFk { get; set; }

    }
}