using Abp.Application.Services.Dto;
using System;

namespace TDV.Rapor.Dtos
{
    public class GetAllTalepsForExcelInput
    {
        public string Filter { get; set; }

        public decimal? MaxTalepMiktarFilter { get; set; }
        public decimal? MinTalepMiktarFilter { get; set; }

        public string OlcuBrFilter { get; set; }

        public decimal? MaxFiyatFilter { get; set; }
        public decimal? MinFiyatFilter { get; set; }

        public decimal? MaxTutarFilter { get; set; }
        public decimal? MinTutarFilter { get; set; }

        public string StokAdiFilter { get; set; }

    }
}