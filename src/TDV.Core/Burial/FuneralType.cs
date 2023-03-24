using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Burial
{
    [Table("FuneralTypes")]
    public class FuneralType : FullAuditedEntity
    {

        [Required]
        [StringLength(FuneralTypeConsts.MaxDescriptionLength, MinimumLength = FuneralTypeConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual bool IsDefault { get; set; }

    }
}