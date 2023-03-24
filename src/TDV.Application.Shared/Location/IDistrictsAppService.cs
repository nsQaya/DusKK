using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Location.Dtos;
using TDV.Dto;
using System.Collections.Generic;

namespace TDV.Location
{
    public interface IDistrictsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDistrictForViewDto>> GetAll(GetAllDistrictsInput input);

        Task<GetDistrictForViewDto> GetDistrictForView(int id);

        Task<GetDistrictForEditOutput> GetDistrictForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDistrictDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetDistrictsToExcel(GetAllDistrictsForExcelInput input);

        Task<List<DistrictLookupTableDto>> GetAllDistrictForTableDropdown(int? cityId = null);


        Task<GetRegionForViewDto> GetRegionFromDistrict(int districtId);

    }
}