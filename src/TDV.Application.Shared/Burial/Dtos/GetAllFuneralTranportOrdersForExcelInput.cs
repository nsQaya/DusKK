using Abp.Application.Services.Dto;
using System;

namespace TDV.Burial.Dtos
{
    public class GetAllFuneralTranportOrdersForExcelInput
    {
        public string Filter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public decimal? MaxStartKMFilter { get; set; }
        public decimal? MinStartKMFilter { get; set; }

        public DateTime? MaxOperationDateFilter { get; set; }
        public DateTime? MinOperationDateFilter { get; set; }

        public decimal? MaxOperationKMFilter { get; set; }
        public decimal? MinOperationKMFilter { get; set; }

        public DateTime? MaxDeliveryDateFilter { get; set; }
        public DateTime? MinDeliveryDateFilter { get; set; }

        public decimal? MaxDeliveryKMFilter { get; set; }
        public decimal? MinDeliveryKMFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public decimal? MaxEndKMFilter { get; set; }
        public decimal? MinEndKMFilter { get; set; }

        public string ReceiverFullNameFilter { get; set; }

        public string ReceiverKinshipDegreeFilter { get; set; }

    }
}