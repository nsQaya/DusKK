using TDV.Communication;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TDV.Communication.Dtos
{
    public class CreateOrEditContactDetailDto : EntityDto<int?>
    {

        public ContactType Type { get; set; }

        [Required]
        [StringLength(ContactDetailConsts.MaxValueLength, MinimumLength = ContactDetailConsts.MinValueLength)]
        public string Value { get; set; }

        public int? ContactId { get; set; }

    }
}