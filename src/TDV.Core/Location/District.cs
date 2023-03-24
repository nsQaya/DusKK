using TDV.Location;
using TDV.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace TDV.Location
{
    [Table("Districts")]
    public class District : FullAuditedEntity
    {

        [Required]
        [StringLength(DistrictConsts.MaxNameLength, MinimumLength = DistrictConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int CityId { get; set; }

        [ForeignKey("CityId")]
        public City CityFk { get; set; }

        public virtual int RegionId { get; set; }

        [ForeignKey("RegionId")]
        public Region RegionFk { get; set; }
        public List<Quarter> Quarters { get; set; }
    }
}