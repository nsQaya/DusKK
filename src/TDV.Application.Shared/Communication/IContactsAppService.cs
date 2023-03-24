using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Communication.Dtos;
using TDV.Dto;

namespace TDV.Communication
{
    public interface IContactsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContactForViewDto>> GetAll(GetAllContactsInput input);

        Task<GetContactForViewDto> GetContactForView(int id);

        Task<GetContactForEditOutput> GetContactForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditContactDto input);

        Task<int> CreateAndGet(CreateOrEditContactDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetContactsToExcel(GetAllContactsForExcelInput input);

    }
}