using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Corporation.Dtos
{
    public class CreateOrEditVehicleDto : EntityDto<int?>
    {

        [Required]
        [StringLength(VehicleConsts.MaxPlateLength, MinimumLength = VehicleConsts.MinPlateLength)]
        public string Plate { get; set; }

        [StringLength(VehicleConsts.MaxDescriptionLength, MinimumLength = VehicleConsts.MinDescriptionLength)]
        public string Description { get; set; }

        public DateTime? EndExaminationDate { get; set; }

        public DateTime? EndInsuranceDate { get; set; }

        public DateTime? EndGuarantyDate { get; set; }

        public int Capactiy { get; set; }

        public int? Year { get; set; }

        [Required]
        [StringLength(VehicleConsts.MaxBrandLength, MinimumLength = VehicleConsts.MinBrandLength)]
        public string Brand { get; set; }

        [StringLength(VehicleConsts.MaxTrackNoLength, MinimumLength = VehicleConsts.MinTrackNoLength)]
        public string TrackNo { get; set; }

        public int CompanyId { get; set; }

    }
}