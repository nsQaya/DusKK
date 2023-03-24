using Abp.Application.Services.Dto;

namespace TDV.Payment.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}