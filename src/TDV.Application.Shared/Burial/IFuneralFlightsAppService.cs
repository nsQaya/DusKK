using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial
{
    public interface IFuneralFlightsAppService : IApplicationService
    {
        Task<PagedResultDto<GetFuneralFlightForViewDto>> GetAll(GetAllFuneralFlightsInput input);

        Task<GetFuneralFlightForViewDto> GetFuneralFlightForView(int id);

        Task<GetFuneralFlightForEditOutput> GetFuneralFlightForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFuneralFlightDto input);
        Task<int> CreateAndGetId(CreateOrEditFuneralFlightDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFuneralFlightsToExcel(GetAllFuneralFlightsForExcelInput input);

        Task<PagedResultDto<FuneralFlightFuneralLookupTableDto>> GetAllFuneralForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<AirlineCompanyLookupTableDto>> GetAllAirlineCompanyForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<AirportLookupTableDto>> GetAllAirportForLookupTable(GetAllForLookupTableInput input);

    }
}