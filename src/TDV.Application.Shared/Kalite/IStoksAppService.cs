using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Kalite.Dtos;
using TDV.Dto;

namespace TDV.Kalite
{
    public interface IStoksAppService : IApplicationService
    {
        Task<PagedResultDto<GetStokForViewDto>> GetAll(GetAllStoksInput input);

        Task<GetStokForViewDto> GetStokForView(int id);

        Task<GetStokForEditOutput> GetStokForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditStokDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetStoksToExcel(GetAllStoksForExcelInput input);

    }
}