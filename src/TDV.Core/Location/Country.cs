using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace TDV.Location
{
    [Table("Countries")]
    public class Country : FullAuditedEntity
    {

        [Required]
        [StringLength(CountryConsts.MaxCodeLength, MinimumLength = CountryConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(CountryConsts.MaxNameLength, MinimumLength = CountryConsts.MinNameLength)]
        public virtual string Name { get; set; }

        public virtual int Order { get; set; }

        public virtual bool IsActive { get; set; }

        public List<City> Cities { get; set; } 

    }
}