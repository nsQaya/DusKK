using TDV.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Location
{
    [Table("Quarters")]
    public class Quarter : FullAuditedEntity
    {

        [Required]
        [StringLength(QuarterConsts.MaxNameLength, MinimumLength = QuarterConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int DistrictId { get; set; }

        [ForeignKey("DistrictId")]
        public District DistrictFk { get; set; }

    }
}