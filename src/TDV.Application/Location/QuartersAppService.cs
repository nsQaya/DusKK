using TDV.Location;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Location.Exporting;
using TDV.Location.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;

namespace TDV.Location
{
    [AbpAuthorize(AppPermissions.Pages_Quarters)]
    public class QuartersAppService : TDVAppServiceBase, IQuartersAppService
    {
        private readonly IRepository<Quarter> _quarterRepository;
        private readonly IQuartersExcelExporter _quartersExcelExporter;
        private readonly IRepository<District, int> _lookup_districtRepository;

        public QuartersAppService(IRepository<Quarter> quarterRepository, IQuartersExcelExporter quartersExcelExporter, IRepository<District, int> lookup_districtRepository)
        {
            _quarterRepository = quarterRepository;
            _quartersExcelExporter = quartersExcelExporter;
            _lookup_districtRepository = lookup_districtRepository;

        }

        public async Task<PagedResultDto<GetQuarterForViewDto>> GetAll(GetAllQuartersInput input)
        {

            var filteredQuarters = _quarterRepository.GetAll()
                        .Include(e => e.DistrictFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictNameFilter), e => e.DistrictFk != null && e.DistrictFk.Name == input.DistrictNameFilter);

            var pagedAndFilteredQuarters = filteredQuarters
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

           
            var totalCount = await filteredQuarters.CountAsync();

            var dbList = await pagedAndFilteredQuarters.ToListAsync();
           
            return new PagedResultDto<GetQuarterForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetQuarterForViewDto>>(dbList)
            );

        }

        public async Task<GetQuarterForViewDto> GetQuarterForView(int id)
        {
            var quarter = await _quarterRepository.GetAsync(id);

            var output = new GetQuarterForViewDto { Quarter = ObjectMapper.Map<QuarterDto>(quarter) };

            if (output.Quarter.DistrictId != null)
            {
                var _lookupDistrict = await _lookup_districtRepository.FirstOrDefaultAsync((int)output.Quarter.DistrictId);
                output.DistrictName = _lookupDistrict?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Quarters_Edit)]
        public async Task<GetQuarterForEditOutput> GetQuarterForEdit(EntityDto input)
        {
            var quarter = await _quarterRepository.GetAllIncluding(x=>x.DistrictFk.CityFk).FirstOrDefaultAsync(x=>x.Id==input.Id);

            var output = new GetQuarterForEditOutput { Quarter = ObjectMapper.Map<CreateOrEditQuarterDto>(quarter) };

            if (output.Quarter.DistrictId != null)
            {
                var _lookupDistrict = await _lookup_districtRepository.FirstOrDefaultAsync((int)output.Quarter.DistrictId);
                output.DistrictName = _lookupDistrict?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditQuarterDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Quarters_Create)]
        protected virtual async Task Create(CreateOrEditQuarterDto input)
        {
            var quarter = ObjectMapper.Map<Quarter>(input);

            await _quarterRepository.InsertAsync(quarter);

        }

        [AbpAuthorize(AppPermissions.Pages_Quarters_Edit)]
        protected virtual async Task Update(CreateOrEditQuarterDto input)
        {
            var quarter = await _quarterRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, quarter);

        }

        [AbpAuthorize(AppPermissions.Pages_Quarters_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _quarterRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetQuartersToExcel(GetAllQuartersForExcelInput input)
        {

            var filteredQuarters = _quarterRepository.GetAll()
                        .Include(e => e.DistrictFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DistrictNameFilter), e => e.DistrictFk != null && e.DistrictFk.Name == input.DistrictNameFilter);

            var query = (from o in filteredQuarters
                         join o1 in _lookup_districtRepository.GetAll() on o.DistrictId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetQuarterForViewDto()
                         {
                             Quarter = new QuarterDto
                             {
                                 Name = o.Name,
                                 Order = o.Order,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             },
                             DistrictName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var quarterListDtos = await query.ToListAsync();

            return _quartersExcelExporter.ExportToFile(quarterListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Quarters)]
        public async Task<List<QuarterLookupTableDto>> GetAllQuartersForTableDropdown(int? districtId = null)
        {
            return await _quarterRepository.GetAll()
                .WhereIf(districtId.HasValue, x => x.DistrictId == districtId.Value)
                .Select(quarter => new QuarterLookupTableDto
                {
                    Id = quarter.Id,
                    DisplayName = quarter == null || quarter.Name == null ? "" : quarter.Name.ToString()
                }).ToListAsync();
        }

    }
}