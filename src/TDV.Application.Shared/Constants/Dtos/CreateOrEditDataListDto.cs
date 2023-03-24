using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Constants.Dtos
{
    public class CreateOrEditDataListDto : EntityDto<int?>
    {

        [Required]
        [StringLength(DataListConsts.MaxCodeLength, MinimumLength = DataListConsts.MinCodeLength)]
        public string Code { get; set; }

        [Required]
        [StringLength(DataListConsts.MaxTypeLength, MinimumLength = DataListConsts.MinTypeLength)]
        public string Type { get; set; }

        [Required]
        [StringLength(DataListConsts.MaxValueLength, MinimumLength = DataListConsts.MinValueLength)]
        public string Value { get; set; }

        public int OrderNumber { get; set; }

        public bool IsActive { get; set; }

    }
}