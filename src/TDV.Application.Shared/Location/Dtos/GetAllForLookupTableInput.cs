using Abp.Application.Services.Dto;

namespace TDV.Location.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}