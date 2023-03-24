using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace TDV.Payment
{
    [Table("FixedPrices")]
    public class FixedPrice : FullAuditedEntity
    {

        [StringLength(FixedPriceConsts.MaxNameLength, MinimumLength = FixedPriceConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [StringLength(FixedPriceConsts.MaxDescriptionLength, MinimumLength = FixedPriceConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public List<FixedPriceDetail> Details { get; set; }
    }
}