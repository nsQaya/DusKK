using TDV.Burial;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Burial
{
    [Table("FuneralTranportOrders")]
    public class FuneralTranportOrder : FullAuditedEntity
    {

        public virtual DateTime? StartDate { get; set; }

        public virtual decimal? StartKM { get; set; }

        public virtual DateTime? OperationDate { get; set; }

        public virtual decimal? OperationKM { get; set; }

        public virtual DateTime? DeliveryDate { get; set; }

        public virtual decimal? DeliveryKM { get; set; }

        public virtual DateTime? EndDate { get; set; }

        public virtual decimal? EndKM { get; set; }

        public virtual string ReceiverFullName { get; set; }

        public virtual string ReceiverKinshipDegree { get; set; }

    }
}