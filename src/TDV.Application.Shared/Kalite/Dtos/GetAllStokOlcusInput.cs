using Abp.Application.Services.Dto;
using System;

namespace TDV.Kalite.Dtos
{
    public class GetAllStokOlcusInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public decimal? MaxAltFilter { get; set; }
        public decimal? MinAltFilter { get; set; }

        public decimal? MaxUstFilter { get; set; }
        public decimal? MinUstFilter { get; set; }

        public string DegerFilter { get; set; }

        public string StokAdiFilter { get; set; }

        public string OlcumOlcuTipiFilter { get; set; }

    }
}