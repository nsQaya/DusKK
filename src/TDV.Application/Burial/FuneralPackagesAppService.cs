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

namespace TDV.Burial
{
    [AbpAuthorize(AppPermissions.Pages_FuneralPackages)]
    public class FuneralPackagesAppService : TDVAppServiceBase, IFuneralPackagesAppService
    {
        private readonly IRepository<FuneralPackage> _funeralPackageRepository;
        private readonly IFuneralPackagesExcelExporter _funeralPackagesExcelExporter;

        public FuneralPackagesAppService(IRepository<FuneralPackage> funeralPackageRepository, IFuneralPackagesExcelExporter funeralPackagesExcelExporter)
        {
            _funeralPackageRepository = funeralPackageRepository;
            _funeralPackagesExcelExporter = funeralPackagesExcelExporter;

        }

        public async Task<PagedResultDto<GetFuneralPackageForViewDto>> GetAll(GetAllFuneralPackagesInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (FuneralStatus)input.StatusFilter
                        : default;

            var filteredFuneralPackages = _funeralPackageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter));

            var pagedAndFilteredFuneralPackages = filteredFuneralPackages
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredFuneralPackages.CountAsync();

            var dbList = await pagedAndFilteredFuneralPackages.ToListAsync();


            return new PagedResultDto<GetFuneralPackageForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFuneralPackageForViewDto>>(dbList)
            );

        }

        public async Task<GetFuneralPackageForViewDto> GetFuneralPackageForView(int id)
        {
            var funeralPackage = await _funeralPackageRepository.GetAsync(id);

            var output = new GetFuneralPackageForViewDto { FuneralPackage = ObjectMapper.Map<FuneralPackageDto>(funeralPackage) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralPackages_Edit)]
        public async Task<GetFuneralPackageForEditOutput> GetFuneralPackageForEdit(EntityDto input)
        {
            var funeralPackage = await _funeralPackageRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFuneralPackageForEditOutput { FuneralPackage = ObjectMapper.Map<CreateOrEditFuneralPackageDto>(funeralPackage) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFuneralPackageDto input)
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

        [AbpAuthorize(AppPermissions.Pages_FuneralPackages_Create)]
        protected virtual async Task Create(CreateOrEditFuneralPackageDto input)
        {
            var funeralPackage = ObjectMapper.Map<FuneralPackage>(input);

            await _funeralPackageRepository.InsertAsync(funeralPackage);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralPackages_Edit)]
        protected virtual async Task Update(CreateOrEditFuneralPackageDto input)
        {
            var funeralPackage = await _funeralPackageRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, funeralPackage);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralPackages_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _funeralPackageRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFuneralPackagesToExcel(GetAllFuneralPackagesForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (FuneralStatus)input.StatusFilter
                        : default;

            var filteredFuneralPackages = _funeralPackageRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter));

            var query = (from o in filteredFuneralPackages
                         select new GetFuneralPackageForViewDto()
                         {
                             FuneralPackage = new FuneralPackageDto
                             {
                                 Status = o.Status,
                                 Code = o.Code,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var funeralPackageListDtos = await query.ToListAsync();

            return _funeralPackagesExcelExporter.ExportToFile(funeralPackageListDtos);
        }

    }
}