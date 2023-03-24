using TDV.Burial;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace TDV.Burial
{
    [Table("FuneralPackages")]
    public class FuneralPackage : FullAuditedEntity
    {

        public virtual FuneralStatus Status { get; set; }

        [Required]
        [StringLength(FuneralPackageConsts.MaxCodeLength, MinimumLength = FuneralPackageConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(FuneralPackageConsts.MaxDescriptionLength, MinimumLength = FuneralPackageConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public List<Funeral> Funerals { get; set; } 

    }
}