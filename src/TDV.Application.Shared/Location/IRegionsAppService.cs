using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Location.Dtos;
using TDV.Dto;
using System.Collections.Generic;

namespace TDV.Location
{
    public interface IRegionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetRegionForViewDto>> GetAll(GetAllRegionsInput input);

        Task<GetRegionForViewDto> GetRegionForView(int id);

        Task<GetRegionForEditOutput> GetRegionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditRegionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetRegionsToExcel(GetAllRegionsForExcelInput input);

        Task<List<FixedPriceLookupTableDto>> GetAllFixedPriceForTableDropdown();

        Task<List<RegionLookupTableDto>> GetAllRegionForTableDropdown();

    }
}