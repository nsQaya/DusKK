using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Payment.Dtos;
using TDV.Dto;

namespace TDV.Payment
{
    public interface IFixedPriceDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetFixedPriceDetailForViewDto>> GetAll(GetAllFixedPriceDetailsInput input);

        Task<GetFixedPriceDetailForViewDto> GetFixedPriceDetailForView(int id);

        Task<GetFixedPriceDetailForEditOutput> GetFixedPriceDetailForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFixedPriceDetailDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFixedPriceDetailsToExcel(GetAllFixedPriceDetailsForExcelInput input);

        Task<PagedResultDto<FixedPriceDetailFixedPriceLookupTableDto>> GetAllFixedPriceForLookupTable(GetAllForLookupTableInput input);

    }
}