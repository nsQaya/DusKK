using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Corporation.Dtos;
using TDV.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace TDV.Corporation
{
    public interface ICompaniesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCompanyForViewDto>> GetAll(GetAllCompaniesInput input);

        Task<GetCompanyForViewDto> GetCompanyForView(int id);

        Task<GetCompanyForEditOutput> GetCompanyForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCompanyDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCompaniesToExcel(GetAllCompaniesForExcelInput input);

        Task<PagedResultDto<CompanyOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input);

        Task<List<CompanyCityLookupTableDto>> GetAllCityForTableDropdown();

        Task<List<CompanyQuarterLookupTableDto>> GetAllQuarterForTableDropdown();

    }
}