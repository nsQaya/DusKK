using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Constants.Dtos;
using TDV.Dto;

namespace TDV.Constants
{
    public interface ICurrenciesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCurrencyForViewDto>> GetAll(GetAllCurrenciesInput input);

        Task<GetCurrencyForViewDto> GetCurrencyForView(int id);

        Task<GetCurrencyForEditOutput> GetCurrencyForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCurrencyDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCurrenciesToExcel(GetAllCurrenciesForExcelInput input);

    }
}