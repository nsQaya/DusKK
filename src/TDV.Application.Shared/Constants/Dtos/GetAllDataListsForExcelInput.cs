using Abp.Application.Services.Dto;
using System;

namespace TDV.Constants.Dtos
{
    public class GetAllDataListsForExcelInput
    {
        public string Filter { get; set; }

        public string TypeFilter { get; set; }

        public string ValueFilter { get; set; }

        public int? MaxOrderNumberFilter { get; set; }
        public int? MinOrderNumberFilter { get; set; }

        public int? IsActiveFilter { get; set; }

    }
}