using TDV.Corporation;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Corporation
{
    [Table("Vehicles")]
    public class Vehicle : FullAuditedEntity
    {

        [Required]
        [StringLength(VehicleConsts.MaxPlateLength, MinimumLength = VehicleConsts.MinPlateLength)]
        public virtual string Plate { get; set; }

        [StringLength(VehicleConsts.MaxDescriptionLength, MinimumLength = VehicleConsts.MinDescriptionLength)]
        public virtual string Description { get; set; }

        public virtual DateTime? EndExaminationDate { get; set; }

        public virtual DateTime? EndInsuranceDate { get; set; }

        public virtual DateTime? EndGuarantyDate { get; set; }

        public virtual int Capactiy { get; set; }

        public virtual int? Year { get; set; }

        [Required]
        [StringLength(VehicleConsts.MaxBrandLength, MinimumLength = VehicleConsts.MinBrandLength)]
        public virtual string Brand { get; set; }

        [StringLength(VehicleConsts.MaxTrackNoLength, MinimumLength = VehicleConsts.MinTrackNoLength)]
        public virtual string TrackNo { get; set; }

        public virtual int CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company CompanyFk { get; set; }

    }
}