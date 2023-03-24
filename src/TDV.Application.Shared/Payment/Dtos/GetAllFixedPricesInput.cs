using Abp.Application.Services.Dto;
using System;

namespace TDV.Payment.Dtos
{
    public class GetAllFixedPricesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

    }
}