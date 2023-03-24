using Abp.Application.Services.Dto;
using System;

namespace TDV.Burial.Dtos
{
    public class GetAllFuneralFlightsForExcelInput
    {
        public string Filter { get; set; }

        public string NoFilter { get; set; }

        public string CodeFilter { get; set; }

        public DateTime? MaxLiftOffDateFilter { get; set; }
        public DateTime? MinLiftOffDateFilter { get; set; }

        public DateTime? MaxLandingDateFilter { get; set; }
        public DateTime? MinLandingDateFilter { get; set; }

        public string FuneralNameFilter { get; set; }

        public string AirlineCompanyCodeFilter { get; set; }

        public string AirportNameFilter { get; set; }

        public string AirportName2Filter { get; set; }

        public int? FuneralIdFilter { get; set; }
    }
}