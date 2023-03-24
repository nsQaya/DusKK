using TDV.Corporation;
using TDV.Communication;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Corporation
{
    [Table("CompanyContacts")]
    public class CompanyContact : FullAuditedEntity
    {

        [Required]
        [StringLength(CompanyContactConsts.MaxTitleLength, MinimumLength = CompanyContactConsts.MinTitleLength)]
        public virtual string Title { get; set; }

        public virtual int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company CompanyFk { get; set; }

        public virtual int? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}