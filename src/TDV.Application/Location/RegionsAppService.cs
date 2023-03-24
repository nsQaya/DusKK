using TDV.Payment;

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
    [AbpAuthorize(AppPermissions.Pages_Regions)]
    public class RegionsAppService : TDVAppServiceBase, IRegionsAppService
    {
        private readonly IRepository<Region> _regionRepository;
        private readonly IRegionsExcelExporter _regionsExcelExporter;
        private readonly IRepository<FixedPrice, int> _lookup_fixedPriceRepository;

        public RegionsAppService(IRepository<Region> regionRepository, IRegionsExcelExporter regionsExcelExporter, IRepository<FixedPrice, int> lookup_fixedPriceRepository)
        {
            _regionRepository = regionRepository;
            _regionsExcelExporter = regionsExcelExporter;
            _lookup_fixedPriceRepository = lookup_fixedPriceRepository;

        }

        public async Task<PagedResultDto<GetRegionForViewDto>> GetAll(GetAllRegionsInput input)
        {

            var filteredRegions = _regionRepository.GetAll()
                        .Include(e => e.FixedPriceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FixedPriceNameFilter), e => e.FixedPriceFk != null && e.FixedPriceFk.Name == input.FixedPriceNameFilter);

            var pagedAndFilteredRegions = filteredRegions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredRegions.CountAsync();

            var dbList = await pagedAndFilteredRegions.ToListAsync();
           
            return new PagedResultDto<GetRegionForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetRegionForViewDto>>(dbList)
            );

        }

        public async Task<GetRegionForViewDto> GetRegionForView(int id)
        {
            var region = await _regionRepository.GetAsync(id);

            var output = new GetRegionForViewDto { Region = ObjectMapper.Map<RegionDto>(region) };

            if (output.Region.FixedPriceId != null)
            {
                var _lookupFixedPrice = await _lookup_fixedPriceRepository.FirstOrDefaultAsync((int)output.Region.FixedPriceId);
                output.FixedPriceName = _lookupFixedPrice?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Regions_Edit)]
        public async Task<GetRegionForEditOutput> GetRegionForEdit(EntityDto input)
        {
            var region = await _regionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetRegionForEditOutput { Region = ObjectMapper.Map<CreateOrEditRegionDto>(region) };

            if (output.Region.FixedPriceId != null)
            {
                var _lookupFixedPrice = await _lookup_fixedPriceRepository.FirstOrDefaultAsync((int)output.Region.FixedPriceId);
                output.FixedPriceName = _lookupFixedPrice?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditRegionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Regions_Create)]
        protected virtual async Task Create(CreateOrEditRegionDto input)
        {
            var region = ObjectMapper.Map<Region>(input);

            await _regionRepository.InsertAsync(region);

        }

        [AbpAuthorize(AppPermissions.Pages_Regions_Edit)]
        protected virtual async Task Update(CreateOrEditRegionDto input)
        {
            var region = await _regionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, region);

        }

        [AbpAuthorize(AppPermissions.Pages_Regions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _regionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetRegionsToExcel(GetAllRegionsForExcelInput input)
        {

            var filteredRegions = _regionRepository.GetAll()
                        .Include(e => e.FixedPriceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FixedPriceNameFilter), e => e.FixedPriceFk != null && e.FixedPriceFk.Name == input.FixedPriceNameFilter);

            var query = (from o in filteredRegions
                         join o1 in _lookup_fixedPriceRepository.GetAll() on o.FixedPriceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetRegionForViewDto()
                         {
                             Region = new RegionDto
                             {
                                 Name = o.Name,
                                 Order = o.Order,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             },
                             FixedPriceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var regionListDtos = await query.ToListAsync();

            return _regionsExcelExporter.ExportToFile(regionListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Regions)]
        public async Task<List<FixedPriceLookupTableDto>> GetAllFixedPriceForTableDropdown()
        {
            return await _lookup_fixedPriceRepository.GetAll()
                .Select(fixedPrice => new FixedPriceLookupTableDto
                {
                    Id = fixedPrice.Id,
                    DisplayName = fixedPrice == null || fixedPrice.Name == null ? "" : fixedPrice.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Regions)]
        public async Task<List<RegionLookupTableDto>> GetAllRegionForTableDropdown()
        {
            return await _regionRepository.GetAll()
               .Select(region => new RegionLookupTableDto
               {
                   Id = region.Id,
                   DisplayName = region.Name
               }).ToListAsync();
        }
    }
}