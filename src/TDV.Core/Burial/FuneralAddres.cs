using TDV.Burial;
using TDV.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Burial
{
    [Table("FuneralAddreses")]
    public class FuneralAddres : FullAuditedEntity
    {

        [StringLength(FuneralAddresConsts.MaxDescriptionLength, MinimumLength = FuneralAddresConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        [Required]
        public virtual string Address { get; set; }

        public virtual int FuneralId { get; set; }

        [ForeignKey("FuneralId")]
        public Funeral FuneralFk { get; set; }

        public virtual int QuarterId { get; set; }

        [ForeignKey("QuarterId")]
        public Quarter QuarterFk { get; set; }

    }
}