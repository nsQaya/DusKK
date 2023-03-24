using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Kalite
{
    [Table("Olcums")]
    public class Olcum : Entity
    {

        [Required]
        public virtual string OlcuTipi { get; set; }

    }
}