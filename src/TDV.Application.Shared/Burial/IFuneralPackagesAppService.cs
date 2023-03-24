using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial
{
    public interface IFuneralPackagesAppService : IApplicationService
    {
        Task<PagedResultDto<GetFuneralPackageForViewDto>> GetAll(GetAllFuneralPackagesInput input);

        Task<GetFuneralPackageForViewDto> GetFuneralPackageForView(int id);

        Task<GetFuneralPackageForEditOutput> GetFuneralPackageForEdit(EntityDto input);

        Task CreateOrEdit(CreateOrEditFuneralPackageDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFuneralPackagesToExcel(GetAllFuneralPackagesForExcelInput input);

    }
}