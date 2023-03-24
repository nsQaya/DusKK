using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Flight.Dtos;
using TDV.Dto;

namespace TDV.Flight
{
    public interface IAirportRegionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAirportRegionForViewDto>> GetAll(GetAllAirportRegionsInput input);

        Task<GetAirportRegionForEditOutput> GetAirportRegionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAirportRegionDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<AirportRegionAirportLookupTableDto>> GetAllAirportForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<AirportRegionRegionLookupTableDto>> GetAllRegionForLookupTable(GetAllForLookupTableInput input);

    }
}