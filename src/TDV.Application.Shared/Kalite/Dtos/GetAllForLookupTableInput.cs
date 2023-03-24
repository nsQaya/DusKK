using Abp.Application.Services.Dto;

namespace TDV.Kalite.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}