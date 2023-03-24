using TDV.Burial;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Burial
{
    [Table("FuneralDocuments")]
    public class FuneralDocument : FullAuditedEntity
    {

        public virtual FuneralDocumentType Type { get; set; }

        [Required]
        public virtual string Path { get; set; }

        public virtual Guid Guid { get; set; }

        public virtual int FuneralId { get; set; }

        [ForeignKey("FuneralId")]
        public Funeral FuneralFk { get; set; }

    }
}