using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Location.Dtos;
using TDV.Dto;
using System.Collections.Generic;

namespace TDV.Location
{
    public interface ICitiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCityForViewDto>> GetAll(GetAllCitiesInput input);

        Task<GetCityForViewDto> GetCityForView(int id);

        Task<GetCityForEditOutput> GetCityForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCityDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input);

        Task<List<CityLookupTableDto>> GetAllCityForTableDropdown(int? countryId = null);

    }
}