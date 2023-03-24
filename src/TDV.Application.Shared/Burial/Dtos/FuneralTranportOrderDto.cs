﻿using System;
using Abp.Application.Services.Dto;

namespace TDV.Burial.Dtos
{
    public class FuneralTranportOrderDto : EntityDto
    {
        public DateTime? StartDate { get; set; }

        public decimal? StartKM { get; set; }

        public DateTime? OperationDate { get; set; }

        public decimal? OperationKM { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal? DeliveryKM { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? EndKM { get; set; }

        public string ReceiverFullName { get; set; }

        public string ReceiverKinshipDegree { get; set; }

        public int FuneralWorkOrderDetailId { get; set; }

    }
}