using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Constants.Dtos;
using TDV.Dto;

namespace TDV.Constants
{
    public interface IDataListsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDataListForViewDto>> GetAll(GetAllDataListsInput input);

        Task<GetDataListForViewDto> GetDataListForView(int id);

        Task<GetDataListForEditOutput> GetDataListForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditDataListDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetDataListsToExcel(GetAllDataListsForExcelInput input);

    }
}