using Abp.Application.Services.Dto;
using System;

namespace TDV.Constants.Dtos
{
    public class GetAllCurrenciesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string CodeFilter { get; set; }

        public string SymbolFilter { get; set; }

        public int? MaxOrderNumberFilter { get; set; }
        public int? MinOrderNumberFilter { get; set; }

        public int? MaxNumberCodeFilter { get; set; }
        public int? MinNumberCodeFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}