using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace TDV.Communication.Dtos
{
    public class ContactDto : EntityDto
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string IdentifyNo { get; set; }

        public ContactNetsisDetailDto NetsisDetail { get; set; }

        public List<ContactDetailDto> Details { get; set; }

    }
}