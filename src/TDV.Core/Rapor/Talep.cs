using TDV.Kalite;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Rapor
{
    [Table("Taleps")]
    public class Talep : Entity
    {

        public virtual decimal TalepMiktar { get; set; }

        public virtual string OlcuBr { get; set; }

        public virtual decimal Fiyat { get; set; }

        public virtual decimal Tutar { get; set; }

        public virtual int StokId { get; set; }

        [ForeignKey("StokId")]
        public Stok StokFk { get; set; }

    }
}