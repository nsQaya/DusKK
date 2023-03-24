using TDV.Communication;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Communication
{
    [Table("ContactDetails")]
    public class ContactDetail : FullAuditedEntity
    {

        public virtual ContactType Type { get; set; }

        [Required]
        [StringLength(ContactDetailConsts.MaxValueLength, MinimumLength = ContactDetailConsts.MinValueLength)]
        public virtual string Value { get; set; }

        public virtual int? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}