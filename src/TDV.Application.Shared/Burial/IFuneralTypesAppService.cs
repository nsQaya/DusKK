using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial
{
    public interface IFuneralTypesAppService : IApplicationService
    {
        Task<PagedResultDto<GetFuneralTypeForViewDto>> GetAll(GetAllFuneralTypesInput input);

        Task<GetFuneralTypeForViewDto> GetFuneralTypeForView(int id);

        Task<GetFuneralTypeForEditOutput> GetFuneralTypeForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFuneralTypeDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFuneralTypesToExcel(GetAllFuneralTypesForExcelInput input);

    }
}