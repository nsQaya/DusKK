using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Kalite
{
    [Table("Stoks")]
    public class Stok : Entity
    {

        [Required]
        public virtual string Kodu { get; set; }

        [Required]
        public virtual string Adi { get; set; }

    }
}