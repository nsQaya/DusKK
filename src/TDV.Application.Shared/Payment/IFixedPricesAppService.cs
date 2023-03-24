using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Payment.Dtos;
using TDV.Dto;

namespace TDV.Payment
{
    public interface IFixedPricesAppService : IApplicationService
    {
        Task<PagedResultDto<GetFixedPriceForViewDto>> GetAll(GetAllFixedPricesInput input);

        Task<GetFixedPriceForViewDto> GetFixedPriceForView(int id);

        Task<GetFixedPriceForEditOutput> GetFixedPriceForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFixedPriceDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFixedPricesToExcel(GetAllFixedPricesForExcelInput input);

    }
}