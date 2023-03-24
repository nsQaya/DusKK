using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Kalite.Dtos;
using TDV.Dto;

namespace TDV.Kalite
{
    public interface IOlcumsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOlcumForViewDto>> GetAll(GetAllOlcumsInput input);

        Task<GetOlcumForViewDto> GetOlcumForView(int id);

        Task<GetOlcumForEditOutput> GetOlcumForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditOlcumDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetOlcumsToExcel(GetAllOlcumsForExcelInput input);

    }
}