using TDV.Kalite;
using TDV.Kalite;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Kalite
{
    [Table("StokOlcus")]
    public class StokOlcu : Entity
    {

        public virtual decimal Alt { get; set; }

        public virtual decimal Ust { get; set; }

        public virtual string Deger { get; set; }

        public virtual int StokId { get; set; }

        [ForeignKey("StokId")]
        public Stok StokFk { get; set; }

        public virtual int OlcumId { get; set; }

        [ForeignKey("OlcumId")]
        public Olcum OlcumFk { get; set; }

    }
}