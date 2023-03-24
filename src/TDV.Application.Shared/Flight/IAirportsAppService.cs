using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Flight.Dtos;
using TDV.Dto;
using System.Collections.Generic;
using TDV.Burial.Dtos;

namespace TDV.Flight
{
    public interface IAirportsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAirportForViewDto>> GetAll(GetAllAirportsInput input);

        Task<GetAirportForViewDto> GetAirportForView(int id);

        Task<GetAirportForEditOutput> GetAirportForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAirportDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAirportsToExcel(GetAllAirportsForExcelInput input);

        Task<List<AirportLookupTableDto>> GetAllAirportForTableDropdown(int? cityId = null);

    }
}