using TDV.Communication;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Communication
{
    [Table("ContactNetsisDetails")]
    public class ContactNetsisDetail : FullAuditedEntity
    {

        [Required]
        [StringLength(ContactNetsisDetailConsts.MaxNetsisNoLength, MinimumLength = ContactNetsisDetailConsts.MinNetsisNoLength)]
        public virtual string NetsisNo { get; set; }

        [Required]
        [StringLength(ContactNetsisDetailConsts.MaxRegistryNoLength, MinimumLength = ContactNetsisDetailConsts.MinRegistryNoLength)]
        public virtual string RegistryNo { get; set; }

        public virtual int ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}