using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace TDV.Communication
{
    [Table("Contacts")]
    public class Contact : FullAuditedEntity
    {

        [Required]
        [StringLength(ContactConsts.MaxNameLength, MinimumLength = ContactConsts.MinNameLength)]
        public virtual string Name { get; set; }

        [Required]
        [StringLength(ContactConsts.MaxSurnameLength, MinimumLength = ContactConsts.MinSurnameLength)]
        public virtual string Surname { get; set; }

        [StringLength(ContactConsts.MaxIdentifyNoLength, MinimumLength = ContactConsts.MinIdentifyNoLength)]
        public virtual string IdentifyNo { get; set; }

        public virtual List<ContactDetail> Details { get; set; }
        public virtual ContactNetsisDetail NetsisDetail { get; set; }

    }
}