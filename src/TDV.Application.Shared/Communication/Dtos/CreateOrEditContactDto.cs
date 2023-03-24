using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace TDV.Communication.Dtos
{
    public class CreateOrEditContactDto : EntityDto<int?>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        [StringLength(ContactConsts.MaxIdentifyNoLength, MinimumLength = ContactConsts.MinIdentifyNoLength)]
        public string IdentifyNo { get; set; }

        public CreateOrEditContactNetsisDetailDto NetsisDetail { get; set; }

        public List<CreateOrEditContactDetailDto> Details { get; set; }

    }
}