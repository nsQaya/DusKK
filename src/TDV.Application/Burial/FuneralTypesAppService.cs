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
    [AbpAuthorize(AppPermissions.Pages_FuneralTypes)]
    public class FuneralTypesAppService : TDVAppServiceBase, IFuneralTypesAppService
    {
        private readonly IRepository<FuneralType> _funeralTypeRepository;
        private readonly IFuneralTypesExcelExporter _funeralTypesExcelExporter;

        public FuneralTypesAppService(IRepository<FuneralType> funeralTypeRepository, IFuneralTypesExcelExporter funeralTypesExcelExporter)
        {
            _funeralTypeRepository = funeralTypeRepository;
            _funeralTypesExcelExporter = funeralTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetFuneralTypeForViewDto>> GetAll(GetAllFuneralTypesInput input)
        {

            var filteredFuneralTypes = _funeralTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsDefaultFilter.HasValue && input.IsDefaultFilter > -1, e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault));

            var pagedAndFilteredFuneralTypes = filteredFuneralTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

           

            var totalCount = await filteredFuneralTypes.CountAsync();

            var dbList = await pagedAndFilteredFuneralTypes.ToListAsync();
          

            return new PagedResultDto<GetFuneralTypeForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFuneralTypeForViewDto>>(dbList)
            );

        }

        public async Task<GetFuneralTypeForViewDto> GetFuneralTypeForView(int id)
        {
            var funeralType = await _funeralTypeRepository.GetAsync(id);

            var output = new GetFuneralTypeForViewDto { FuneralType = ObjectMapper.Map<FuneralTypeDto>(funeralType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralTypes_Edit)]
        public async Task<GetFuneralTypeForEditOutput> GetFuneralTypeForEdit(EntityDto input)
        {
            var funeralType = await _funeralTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFuneralTypeForEditOutput { FuneralType = ObjectMapper.Map<CreateOrEditFuneralTypeDto>(funeralType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFuneralTypeDto input)
        {
            if (input.IsDefault)
            {
                await _funeralTypeRepository.GetAll().Where(x => x.IsDefault == true).ForEachAsync(async (item) =>
                {
                    item.IsDefault = false;
                    await _funeralTypeRepository.UpdateAsync(item);
                });
            }

            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralTypes_Create)]
        protected virtual async Task Create(CreateOrEditFuneralTypeDto input)
        {
            var funeralType = ObjectMapper.Map<FuneralType>(input);

            await _funeralTypeRepository.InsertAsync(funeralType);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralTypes_Edit)]
        protected virtual async Task Update(CreateOrEditFuneralTypeDto input)
        {
            var funeralType = await _funeralTypeRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, funeralType);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralTypes_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _funeralTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFuneralTypesToExcel(GetAllFuneralTypesForExcelInput input)
        {

            var filteredFuneralTypes = _funeralTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.IsDefaultFilter.HasValue && input.IsDefaultFilter > -1, e => (input.IsDefaultFilter == 1 && e.IsDefault) || (input.IsDefaultFilter == 0 && !e.IsDefault));

            var query = (from o in filteredFuneralTypes
                         select new GetFuneralTypeForViewDto()
                         {
                             FuneralType = new FuneralTypeDto
                             {
                                 Description = o.Description,
                                 IsDefault = o.IsDefault,
                                 Id = o.Id
                             }
                         });

            var funeralTypeListDtos = await query.ToListAsync();

            return _funeralTypesExcelExporter.ExportToFile(funeralTypeListDtos);
        }

    }
}