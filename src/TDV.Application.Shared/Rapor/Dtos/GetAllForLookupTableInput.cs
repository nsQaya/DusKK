using Abp.Application.Services.Dto;

namespace TDV.Rapor.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}