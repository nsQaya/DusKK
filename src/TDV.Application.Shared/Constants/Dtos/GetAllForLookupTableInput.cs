using Abp.Application.Services.Dto;

namespace TDV.Constants.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}