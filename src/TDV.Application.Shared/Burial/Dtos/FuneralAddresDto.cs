using System;
using Abp.Application.Services.Dto;

namespace TDV.Burial.Dtos
{
    public class FuneralAddresDto : EntityDto
    {
        public string Description { get; set; }

        public string Address { get; set; }

        public int FuneralId { get; set; }

        public int QuarterId { get; set; }

    }
}