using Abp.Application.Services.Dto;
using System;

namespace TDV.Payment.Dtos
{
    public class GetAllContractFormulesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string FormuleFilter { get; set; }

    }
}