using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Location.Dtos;
using TDV.Dto;
using System.Collections.Generic;

namespace TDV.Location
{
    public interface IQuartersAppService : IApplicationService
    {
        Task<PagedResultDto<GetQuarterForViewDto>> GetAll(GetAllQuartersInput input);

        Task<GetQuarterForViewDto> GetQuarterForView(int id);

        Task<GetQuarterForEditOutput> GetQuarterForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditQuarterDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetQuartersToExcel(GetAllQuartersForExcelInput input);

        Task<List<QuarterLookupTableDto>> GetAllQuartersForTableDropdown(int? districtId = null);

    }
}