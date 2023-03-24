using TDV.Authorization.Users;
using TDV.Communication;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Authorization
{
    [Table("UserDetails")]
    public class UserDetail : FullAuditedEntity
    {

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public User UserFk { get; set; }

        public virtual int? ContactId { get; set; }

        [ForeignKey("ContactId")]
        public Contact ContactFk { get; set; }

    }
}