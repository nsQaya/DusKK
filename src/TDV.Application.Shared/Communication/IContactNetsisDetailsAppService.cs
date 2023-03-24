using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Communication.Dtos;
using TDV.Dto;

namespace TDV.Communication
{
    public interface IContactNetsisDetailsAppService : IApplicationService
    {
        Task<PagedResultDto<GetContactNetsisDetailForViewDto>> GetAll(GetAllContactNetsisDetailsInput input);

        Task<GetContactNetsisDetailForEditOutput> GetContactNetsisDetailForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditContactNetsisDetailDto input);

        Task Delete(EntityDto input);

        Task<PagedResultDto<ContactNetsisDetailContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input);

    }
}