using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace TDV.Constants
{
    [Table("DataLists")]
    public class DataList : Entity
    {

        [Required]
        [StringLength(DataListConsts.MaxCodeLength, MinimumLength = DataListConsts.MinCodeLength)]
        public virtual string Code { get; set; }

        [Required]
        [StringLength(DataListConsts.MaxTypeLength, MinimumLength = DataListConsts.MinTypeLength)]
        public virtual string Type { get; set; }

        [Required]
        [StringLength(DataListConsts.MaxValueLength, MinimumLength = DataListConsts.MinValueLength)]
        public virtual string Value { get; set; }

        public virtual int OrderNumber { get; set; }

        public virtual bool IsActive { get; set; }

    }
}