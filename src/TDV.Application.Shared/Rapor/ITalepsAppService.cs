using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Rapor.Dtos;
using TDV.Dto;
using System.Collections.Generic;

namespace TDV.Rapor
{
    public interface ITalepsAppService : IApplicationService
    {
        Task<PagedResultDto<GetTalepForViewDto>> GetAll(GetAllTalepsInput input);

        Task<GetTalepForViewDto> GetTalepForView(int id);

        Task<GetTalepForEditOutput> GetTalepForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditTalepDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetTalepsToExcel(GetAllTalepsForExcelInput input);

        Task<List<TalepStokLookupTableDto>> GetAllStokForTableDropdown();

    }
}