using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial
{
    public interface IFuneralTranportOrdersAppService : IApplicationService
    {
        Task<PagedResultDto<GetFuneralTranportOrderForViewDto>> GetAll(GetAllFuneralTranportOrdersInput input);

        Task<GetFuneralTranportOrderForViewDto> GetFuneralTranportOrderForView(int id);

        Task<GetFuneralTranportOrderForEditOutput> GetFuneralTranportOrderForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFuneralTranportOrderDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFuneralTranportOrdersToExcel(GetAllFuneralTranportOrdersForExcelInput input);

    }
}