using TDV.Communication;

using System;
using Abp.Application.Services.Dto;

namespace TDV.Communication.Dtos
{
    public class ContactDetailDto : EntityDto
    {
        public ContactType Type { get; set; }

        public string Value { get; set; }

        public int? ContactId { get; set; }

    }
}