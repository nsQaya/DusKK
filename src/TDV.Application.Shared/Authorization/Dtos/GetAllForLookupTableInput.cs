using Abp.Application.Services.Dto;

namespace TDV.Authorization.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}