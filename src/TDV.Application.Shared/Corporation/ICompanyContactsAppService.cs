using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Corporation.Dtos;
using TDV.Dto;

namespace TDV.Corporation
{
    public interface ICompanyContactsAppService : IApplicationService
    {
        Task<PagedResultDto<GetCompanyContactForViewDto>> GetAll(GetAllCompanyContactsInput input);

        Task<GetCompanyContactForViewDto> GetCompanyContactForView(int id);

        Task<GetCompanyContactForEditOutput> GetCompanyContactForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditCompanyContactDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetCompanyContactsToExcel(GetAllCompanyContactsForExcelInput input);

        Task<PagedResultDto<CompanyContactCompanyLookupTableDto>> GetAllCompanyForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<CompanyContactContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}