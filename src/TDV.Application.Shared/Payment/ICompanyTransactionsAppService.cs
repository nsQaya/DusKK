using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Payment.Dtos;
using TDV.Dto;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Collections.Generic;

namespace TDV.Payment
{
    public interface ICompanyTransactionsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCompanyTransactionForViewDto>> GetAll(GetAllCompanyTransactionsInput input);

        Task<GetCompanyTransactionForViewDto> GetCompanyTransactionForView(int id);

        Task<GetCompanyTransactionForEditOutput> GetCompanyTransactionForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCompanyTransactionDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCompanyTransactionsToExcel(GetAllCompanyTransactionsForExcelInput input);

        Task<List<CompanyTransactionCompanyLookupTableDto>> GetAllCompanyForTableDropdown();

        Task<List<CompanyTransactionFuneralLookupTableDto>> GetAllFuneralForTableDropdown();

        Task<List<CompanyTransactionDataListLookupTableDto>> GetAllDataListForTableDropdown();

        Task<List<CompanyTransactionCurrencyLookupTableDto>> GetAllCurrencyForTableDropdown();

    }
}