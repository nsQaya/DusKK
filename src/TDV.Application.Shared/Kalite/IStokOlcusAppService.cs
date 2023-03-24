using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Kalite.Dtos;
using TDV.Dto;
using System.Collections.Generic;

namespace TDV.Kalite
{
    public interface IStokOlcusAppService : IApplicationService
    {
        Task<PagedResultDto<GetStokOlcuForViewDto>> GetAll(GetAllStokOlcusInput input);

        Task<GetStokOlcuForViewDto> GetStokOlcuForView(int id);

        Task<GetStokOlcuForEditOutput> GetStokOlcuForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditStokOlcuDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetStokOlcusToExcel(GetAllStokOlcusForExcelInput input);

        Task<PagedResultDto<StokOlcuStokLookupTableDto>> GetAllStokForLookupTable(GetAllForLookupTableInput input);

        Task<List<StokOlcuOlcumLookupTableDto>> GetAllOlcumForTableDropdown();

    }
}