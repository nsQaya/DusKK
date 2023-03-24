using System;
using Abp.Application.Services.Dto;

namespace TDV.Rapor.Dtos
{
    public class TalepDto : EntityDto
    {
        public decimal TalepMiktar { get; set; }

        public string OlcuBr { get; set; }

        public decimal Fiyat { get; set; }

        public decimal Tutar { get; set; }

        public int StokId { get; set; }

    }
}