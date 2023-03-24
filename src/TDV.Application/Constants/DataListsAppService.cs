using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Constants.Exporting;
using TDV.Constants.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;

namespace TDV.Constants
{
    [AbpAuthorize(AppPermissions.Pages_DataLists)]
    public class DataListsAppService : TDVAppServiceBase, IDataListsAppService
    {
        private readonly IRepository<DataList> _dataListRepository;
        private readonly IDataListsExcelExporter _dataListsExcelExporter;

        public DataListsAppService(IRepository<DataList> dataListRepository, IDataListsExcelExporter dataListsExcelExporter)
        {
            _dataListRepository = dataListRepository;
            _dataListsExcelExporter = dataListsExcelExporter;

        }

        public async Task<PagedResultDto<GetDataListForViewDto>> GetAll(GetAllDataListsInput input)
        {

            var filteredDataLists = _dataListRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Type.Contains(input.Filter) || e.Value.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter), e => e.Value.Contains(input.ValueFilter))
                        .WhereIf(input.MinOrderNumberFilter != null, e => e.OrderNumber >= input.MinOrderNumberFilter)
                        .WhereIf(input.MaxOrderNumberFilter != null, e => e.OrderNumber <= input.MaxOrderNumberFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredDataLists = filteredDataLists
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredDataLists.CountAsync();

            var dbList = await pagedAndFilteredDataLists.ToListAsync();
            var results = new List<GetDataListForViewDto>();

            return new PagedResultDto<GetDataListForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetDataListForViewDto>>(dbList)
            );

        }

        public async Task<GetDataListForViewDto> GetDataListForView(int id)
        {
            var dataList = await _dataListRepository.GetAsync(id);

            var output = new GetDataListForViewDto { DataList = ObjectMapper.Map<DataListDto>(dataList) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DataLists_Edit)]
        public async Task<GetDataListForEditOutput> GetDataListForEdit(EntityDto input)
        {
            var dataList = await _dataListRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDataListForEditOutput { DataList = ObjectMapper.Map<CreateOrEditDataListDto>(dataList) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDataListDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DataLists_Create)]
        protected virtual async Task Create(CreateOrEditDataListDto input)
        {
            var dataList = ObjectMapper.Map<DataList>(input);

            await _dataListRepository.InsertAsync(dataList);

        }

        [AbpAuthorize(AppPermissions.Pages_DataLists_Edit)]
        protected virtual async Task Update(CreateOrEditDataListDto input)
        {
            var dataList = await _dataListRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, dataList);

        }

        [AbpAuthorize(AppPermissions.Pages_DataLists_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _dataListRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDataListsToExcel(GetAllDataListsForExcelInput input)
        {

            var filteredDataLists = _dataListRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Type.Contains(input.Filter) || e.Value.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TypeFilter), e => e.Type.Contains(input.TypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter), e => e.Value.Contains(input.ValueFilter))
                        .WhereIf(input.MinOrderNumberFilter != null, e => e.OrderNumber >= input.MinOrderNumberFilter)
                        .WhereIf(input.MaxOrderNumberFilter != null, e => e.OrderNumber <= input.MaxOrderNumberFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredDataLists
                         select new GetDataListForViewDto()
                         {
                             DataList = new DataListDto
                             {
                                 Type = o.Type,
                                 Value = o.Value,
                                 OrderNumber = o.OrderNumber,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var dataListListDtos = await query.ToListAsync();

            return _dataListsExcelExporter.ExportToFile(dataListListDtos);
        }

    }
}