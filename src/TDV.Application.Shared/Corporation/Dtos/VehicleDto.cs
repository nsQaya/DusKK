using System;
using Abp.Application.Services.Dto;

namespace TDV.Corporation.Dtos
{
    public class VehicleDto : EntityDto
    {
        public string Plate { get; set; }

        public string Description { get; set; }

        public DateTime? EndExaminationDate { get; set; }

        public DateTime? EndInsuranceDate { get; set; }

        public DateTime? EndGuarantyDate { get; set; }

        public int Capactiy { get; set; }

        public int? Year { get; set; }

        public string Brand { get; set; }

        public string TrackNo { get; set; }

        public int CompanyId { get; set; }

    }
}