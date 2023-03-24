using TDV.Location;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace TDV.Location
{
    [Table("Cities")]
    public class City : FullAuditedEntity
    {

        [StringLength(CityConsts.MaxCodeLength, MinimumLength = CityConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(CityConsts.MaxNameLength, MinimumLength = CityConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Country CountryFk { get; set; }
        public virtual List<District> Districts { get; set; }
    }
}