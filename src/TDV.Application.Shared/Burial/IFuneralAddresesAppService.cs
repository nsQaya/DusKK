using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial
{
    public interface IFuneralAddresesAppService : IApplicationService
    {
        Task<PagedResultDto<GetFuneralAddresForViewDto>> GetAll(GetAllFuneralAddresesInput input);

        Task<GetFuneralAddresForViewDto> GetFuneralAddresForView(int id);

        Task<GetFuneralAddresForEditOutput> GetFuneralAddresForEdit(EntityDto input);

        Task<GetFuneralAddresForEditOutput> GetFuneralAddresForStep(int funeralId);

        Task CreateOrEdit(CreateOrEditFuneralAddresDto input);
        Task<int> CreateAndGetId(CreateOrEditFuneralAddresDto input);
        Task Delete(EntityDto input);

        Task<FileDto> GetFuneralAddresesToExcel(GetAllFuneralAddresesForExcelInput input);

        Task<PagedResultDto<FuneralAddresFuneralLookupTableDto>> GetAllFuneralForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<FuneralAddresQuarterLookupTableDto>> GetAllQuarterForLookupTable(GetAllForLookupTableInput input);

    }
}