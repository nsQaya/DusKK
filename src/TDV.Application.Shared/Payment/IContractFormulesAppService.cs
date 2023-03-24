using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Payment.Dtos;
using TDV.Dto;

namespace TDV.Payment
{
    public interface IContractFormulesAppService : IApplicationService
    {
        Task<PagedResultDto<GetContractFormuleForViewDto>> GetAll(GetAllContractFormulesInput input);

        Task<GetContractFormuleForViewDto> GetContractFormuleForView(int id);

        Task<GetContractFormuleForEditOutput> GetContractFormuleForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditContractFormuleDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetContractFormulesToExcel(GetAllContractFormulesForExcelInput input);

    }
}