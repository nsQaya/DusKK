using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Communication.Dtos;
using TDV.Dto;

namespace TDV.Communication
{
    public interface IContactDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContactDetailForViewDto>> GetAll(GetAllContactDetailsInput input);

        Task<GetContactDetailForViewDto> GetContactDetailForView(int id);

        Task<GetContactDetailForEditOutput> GetContactDetailForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditContactDetailDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetContactDetailsToExcel(GetAllContactDetailsForExcelInput input);

        Task<PagedResultDto<ContactDetailContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}