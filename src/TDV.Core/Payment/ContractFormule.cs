using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Payment
{
    [Table("ContractFormules")]
    public class ContractFormule : FullAuditedEntity
    {

        [Required]
        public virtual string Formule { get; set; }

        [StringLength(ContractFormuleConsts.MaxDescriptionLength, MinimumLength = ContractFormuleConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

    }
}