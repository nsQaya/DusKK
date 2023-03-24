using System;
using Abp.Application.Services.Dto;

namespace TDV.Kalite.Dtos
{
    public class StokDto : EntityDto
    {
        public string Kodu { get; set; }

        public string Adi { get; set; }

    }
}