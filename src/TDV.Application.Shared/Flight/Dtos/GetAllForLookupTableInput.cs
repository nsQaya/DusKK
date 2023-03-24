using Abp.Application.Services.Dto;

namespace TDV.Flight.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public int CountryId { get; set; }
    }
}