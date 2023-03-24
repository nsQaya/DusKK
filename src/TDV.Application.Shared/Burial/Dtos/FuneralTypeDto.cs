using System;
using Abp.Application.Services.Dto;

namespace TDV.Burial.Dtos
{
    public class FuneralTypeDto : EntityDto
    {
        public string Description { get; set; }

        public bool IsDefault { get; set; }

    }
}