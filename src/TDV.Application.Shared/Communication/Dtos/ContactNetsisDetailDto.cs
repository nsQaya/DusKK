using System;
using Abp.Application.Services.Dto;

namespace TDV.Communication.Dtos
{
    public class ContactNetsisDetailDto : EntityDto
    {
        public string NetsisNo { get; set; }

        public string RegistryNo { get; set; }

        public int ContactId { get; set; }

    }
}