using TDV.Burial;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Burial.Exporting;
using TDV.Burial.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;
using TDV.Integration.Modules.Interfaces;

namespace TDV.Burial
{
    [AbpAuthorize(AppPermissions.Pages_FuneralDocuments)]
    public class FuneralDocumentsAppService : TDVAppServiceBase, IFuneralDocumentsAppService
    {
        private readonly IRepository<FuneralDocument> _funeralDocumentRepository;
        private readonly IFuneralDocumentsExcelExporter _funeralDocumentsExcelExporter;
        private readonly IRepository<Funeral, int> _lookup_funeralRepository;
        private readonly IBlobStorageModule _blobStorage;

        public FuneralDocumentsAppService(
            IRepository<FuneralDocument> funeralDocumentRepository,
            IFuneralDocumentsExcelExporter funeralDocumentsExcelExporter,
            IRepository<Funeral, int> lookup_funeralRepository,
            IBlobStorageModule blobStorage)
        {
            _funeralDocumentRepository = funeralDocumentRepository;
            _funeralDocumentsExcelExporter = funeralDocumentsExcelExporter;
            _lookup_funeralRepository = lookup_funeralRepository;
            _blobStorage = blobStorage;
        }

        public async Task<PagedResultDto<GetFuneralDocumentForViewDto>> GetAll(GetAllFuneralDocumentsInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                        ? (FuneralDocumentType)input.TypeFilter
                        : default;

            var filteredFuneralDocuments = _funeralDocumentRepository.GetAll()
                        .Include(e => e.FuneralFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Path.Contains(input.Filter))
                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PathFilter), e => e.Path.Contains(input.PathFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GuidFilter.ToString()), e => e.Guid.ToString() == input.GuidFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.FuneralFk == null || e.FuneralFk.TransferNo == null ? "" : e.FuneralFk.TransferNo.ToString()
, e.FuneralFk == null || e.FuneralFk.Name == null ? "" : e.FuneralFk.Name.ToString()
, e.FuneralFk == null || e.FuneralFk.Surname == null ? "" : e.FuneralFk.Surname.ToString()
) == input.FuneralDisplayPropertyFilter);

            var pagedAndFilteredFuneralDocuments = filteredFuneralDocuments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredFuneralDocuments.CountAsync();

            var dbList = await pagedAndFilteredFuneralDocuments.ToListAsync();

            return new PagedResultDto<GetFuneralDocumentForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFuneralDocumentForViewDto>>(dbList)
            );

        }

        public async Task<GetFuneralDocumentForViewDto> GetFuneralDocumentForView(int id)
        {
            var funeralDocument = await _funeralDocumentRepository.GetAsync(id);

            var output = new GetFuneralDocumentForViewDto { FuneralDocument = ObjectMapper.Map<FuneralDocumentDto>(funeralDocument) };

            if (output.FuneralDocument.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.FuneralDocument.FuneralId);
                output.FuneralDisplayProperty = string.Format("{0} {1} {2}", _lookupFuneral.TransferNo, _lookupFuneral.Name, _lookupFuneral.Surname);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralDocuments_Edit)]
        public async Task<GetFuneralDocumentForEditOutput> GetFuneralDocumentForEdit(EntityDto input)
        {
            var funeralDocument = await _funeralDocumentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFuneralDocumentForEditOutput { FuneralDocument = ObjectMapper.Map<CreateOrEditFuneralDocumentDto>(funeralDocument) };

            if (output.FuneralDocument.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.FuneralDocument.FuneralId);
                output.FuneralDisplayProperty = string.Format("{0} {1} {2}", _lookupFuneral.TransferNo, _lookupFuneral.Name, _lookupFuneral.Surname);
            }

            return output;
        }
        [AbpAuthorize(AppPermissions.Pages_FuneralDocuments_Edit)]
        public async Task<List<CreateOrEditFuneralDocumentDto>> GetFuneralDocumentsForStep(int funeralId)
        {
            var funeralDocument = await _funeralDocumentRepository.GetAll().Where(x => x.FuneralId == funeralId).ToListAsync();
            return ObjectMapper.Map<List<CreateOrEditFuneralDocumentDto>>(funeralDocument);
        }
        public async Task CreateOrEdit(CreateOrEditFuneralDocumentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralDocuments_Create)]
        protected virtual async Task Create(CreateOrEditFuneralDocumentDto input)
        {
            var funeralDocument = ObjectMapper.Map<FuneralDocument>(input);

            await _funeralDocumentRepository.InsertAsync(funeralDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralDocuments_Create)]
        public async Task<int> CreateAndGetId(CreateOrEditFuneralDocumentDto input)
        {
            var funeralDocument = ObjectMapper.Map<FuneralDocument>(input);
            return await _funeralDocumentRepository.InsertAndGetIdAsync(funeralDocument);
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralDocuments_Edit)]
        protected virtual async Task Update(CreateOrEditFuneralDocumentDto input)
        {
            var funeralDocument = await _funeralDocumentRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, funeralDocument);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralDocuments_Delete)]
        public async Task Delete(EntityDto input)
        {
            var funeralDocument = await _funeralDocumentRepository.FirstOrDefaultAsync((int)input.Id);
            await _blobStorage.DeleteDocumentBlob(funeralDocument.Guid.ToString("N"));
            await _funeralDocumentRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFuneralDocumentsToExcel(GetAllFuneralDocumentsForExcelInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                        ? (FuneralDocumentType)input.TypeFilter
                        : default;

            var filteredFuneralDocuments = _funeralDocumentRepository.GetAll()
                        .Include(e => e.FuneralFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Path.Contains(input.Filter))
                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PathFilter), e => e.Path.Contains(input.PathFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.GuidFilter.ToString()), e => e.Guid.ToString() == input.GuidFilter.ToString())
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.FuneralFk == null || e.FuneralFk.TransferNo == null ? "" : e.FuneralFk.TransferNo.ToString()
, e.FuneralFk == null || e.FuneralFk.Name == null ? "" : e.FuneralFk.Name.ToString()
, e.FuneralFk == null || e.FuneralFk.Surname == null ? "" : e.FuneralFk.Surname.ToString()
) == input.FuneralDisplayPropertyFilter);

            var query = (from o in filteredFuneralDocuments
                         join o1 in _lookup_funeralRepository.GetAll() on o.FuneralId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetFuneralDocumentForViewDto()
                         {
                             FuneralDocument = new FuneralDocumentDto
                             {
                                 Type = o.Type,
                                 Path = o.Path,
                                 Guid = o.Guid,
                                 Id = o.Id
                             },
                             FuneralDisplayProperty = string.Format("{0} {1} {2}", s1 == null || s1.TransferNo == null ? "" : s1.TransferNo.ToString()
, s1 == null || s1.Name == null ? "" : s1.Name.ToString()
, s1 == null || s1.Surname == null ? "" : s1.Surname.ToString()
)
                         });

            var funeralDocumentListDtos = await query.ToListAsync();

            return _funeralDocumentsExcelExporter.ExportToFile(funeralDocumentListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralDocuments)]
        public async Task<PagedResultDto<FuneralDocumentFuneralLookupTableDto>> GetAllFuneralForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_funeralRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => string.Format("{0} {1} {2}", e.TransferNo, e.Name, e.Surname).Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var funeralList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FuneralDocumentFuneralLookupTableDto>();
            foreach (var funeral in funeralList)
            {
                lookupTableDtoList.Add(new FuneralDocumentFuneralLookupTableDto
                {
                    Id = funeral.Id,
                    DisplayName = string.Format("{0} {1} {2}", funeral.TransferNo, funeral.Name, funeral.Surname)
                });
            }

            return new PagedResultDto<FuneralDocumentFuneralLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_FuneralDocuments_Edit)]
        public async Task RemovePathFile(EntityDto input)
        {
            var funeralDocument = await _funeralDocumentRepository.FirstOrDefaultAsync(input.Id);
            if (funeralDocument == null)
            {
                throw new UserFriendlyException(L("EntityNotFound"));
            }

            await _blobStorage.DeleteDocumentBlob(funeralDocument.Guid.ToString("N"));
            funeralDocument.Path = null;
           
        }
    }
}