using Abp.Application.Services.Dto;
using System;

namespace TDV.Corporation.Dtos
{
    public class GetAllVehiclesForExcelInput
    {
        public string Filter { get; set; }

        public string PlateFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public DateTime? MaxEndExaminationDateFilter { get; set; }
        public DateTime? MinEndExaminationDateFilter { get; set; }

        public DateTime? MaxEndInsuranceDateFilter { get; set; }
        public DateTime? MinEndInsuranceDateFilter { get; set; }

        public DateTime? MaxEndGuarantyDateFilter { get; set; }
        public DateTime? MinEndGuarantyDateFilter { get; set; }

        public int? MaxCapactiyFilter { get; set; }
        public int? MinCapactiyFilter { get; set; }

        public int? MaxYearFilter { get; set; }
        public int? MinYearFilter { get; set; }

        public string BrandFilter { get; set; }

        public string TrackNoFilter { get; set; }

        public string CompanyDisplayPropertyFilter { get; set; }

    }
}