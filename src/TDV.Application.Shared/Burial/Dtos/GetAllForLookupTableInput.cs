using Abp.Application.Services.Dto;

namespace TDV.Burial.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}