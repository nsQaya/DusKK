using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TDV.Burial.Dtos;
using TDV.Dto;

namespace TDV.Burial
{
    public interface IFuneralDocumentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetFuneralDocumentForViewDto>> GetAll(GetAllFuneralDocumentsInput input);

        Task<GetFuneralDocumentForViewDto> GetFuneralDocumentForView(int id);

        Task<GetFuneralDocumentForEditOutput> GetFuneralDocumentForEdit(EntityDto input);

        Task<List<CreateOrEditFuneralDocumentDto>> GetFuneralDocumentsForStep(int funeralId);

        Task CreateOrEdit(CreateOrEditFuneralDocumentDto input);

        Task<int> CreateAndGetId(CreateOrEditFuneralDocumentDto input);

        Task Delete(EntityDto input);

        Task<FileDto> GetFuneralDocumentsToExcel(GetAllFuneralDocumentsForExcelInput input);

        Task<PagedResultDto<FuneralDocumentFuneralLookupTableDto>> GetAllFuneralForLookupTable(GetAllForLookupTableInput input);

        Task RemovePathFile(EntityDto input);
    }
}