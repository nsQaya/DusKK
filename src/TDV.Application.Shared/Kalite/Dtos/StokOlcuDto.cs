using System;
using Abp.Application.Services.Dto;

namespace TDV.Kalite.Dtos
{
    public class StokOlcuDto : EntityDto
    {
        public decimal Alt { get; set; }

        public decimal Ust { get; set; }

        public string Deger { get; set; }

        public int StokId { get; set; }

        public int OlcumId { get; set; }

    }
}