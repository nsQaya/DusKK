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
    public interface IAirlineCompaniesAppService : IApplicationService
    {
        Task<PagedResultDto<GetAirlineCompanyForViewDto>> GetAll(GetAllAirlineCompaniesInput input);

        Task<GetAirlineCompanyForViewDto> GetAirlineCompanyForView(int id);

        Task<GetAirlineCompanyForEditOutput> GetAirlineCompanyForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditAirlineCompanyDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetAirlineCompaniesToExcel(GetAllAirlineCompaniesForExcelInput input);

        Task<List<AirlineCompanyLookupTableDto>> GetAllAirlineCompanyForTableDropdown();
    }
}