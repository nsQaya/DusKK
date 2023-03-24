using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Authorization.Dtos;
using TDV.Dto;

namespace TDV.Authorization
{
    public interface IUserDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetUserDetailForViewDto>> GetAll(GetAllUserDetailsInput input);

        Task<GetUserDetailForViewDto> GetUserDetailForView(int id);

        Task<GetUserDetailForEditOutput> GetUserDetailForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditUserDetailDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetUserDetailsToExcel(GetAllUserDetailsForExcelInput input);

        Task<PagedResultDto<UserDetailUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<PagedResultDto<UserDetailContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}