using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Payment.Dtos;
using TDV.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace TDV.Payment
{
    public interface IContractsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContractForViewDto>> GetAll(GetAllContractsInput input);

        Task<GetContractForViewDto> GetContractForView(int id);

        Task<GetContractForEditOutput> GetContractForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditContractDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetContractsToExcel(GetAllContractsForExcelInput input);

        Task<List<ContractRegionLookupTableDto>> GetAllRegionForTableDropdown();

        Task<List<ContractCompanyLookupTableDto>> GetAllCompanyForTableDropdown();

    }
}