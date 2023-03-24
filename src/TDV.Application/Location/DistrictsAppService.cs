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
    [AbpAuthorize(AppPermissions.Pages_Districts)]
    public class DistrictsAppService : TDVAppServiceBase, IDistrictsAppService
    {
        private readonly IRepository<District> _districtRepository;
        private readonly IDistrictsExcelExporter _districtsExcelExporter;
        private readonly IRepository<City, int> _lookup_cityRepository;
        private readonly IRepository<Region, int> _lookup_regionRepository;

        public DistrictsAppService(IRepository<District> districtRepository, IDistrictsExcelExporter districtsExcelExporter, IRepository<City, int> lookup_cityRepository, IRepository<Region, int> lookup_regionRepository)
        {
            _districtRepository = districtRepository;
            _districtsExcelExporter = districtsExcelExporter;
            _lookup_cityRepository = lookup_cityRepository;
            _lookup_regionRepository = lookup_regionRepository;

        }

        public async Task<PagedResultDto<GetDistrictForViewDto>> GetAll(GetAllDistrictsInput input)
        {

            var filteredDistricts = _districtRepository.GetAll()
                        .Include(e => e.CityFk)
                        .Include(e => e.RegionFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityDisplayPropertyFilter), e => string.Format("{0} {1}", e.CityFk == null || e.CityFk.Code == null ? "" : e.CityFk.Code.ToString()
, e.CityFk == null || e.CityFk.Name == null ? "" : e.CityFk.Name.ToString()
) == input.CityDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionNameFilter), e => e.RegionFk != null && e.RegionFk.Name == input.RegionNameFilter);

            var pagedAndFilteredDistricts = filteredDistricts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredDistricts.CountAsync();

            var dbList = await pagedAndFilteredDistricts.ToListAsync();
           
            return new PagedResultDto<GetDistrictForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetDistrictForViewDto>>(dbList)
            );

        }

        public async Task<GetDistrictForViewDto> GetDistrictForView(int id)
        {
            var district = await _districtRepository.GetAsync(id);

            var output = new GetDistrictForViewDto { District = ObjectMapper.Map<DistrictDto>(district) };

            if (output.District.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((int)output.District.CityId);
                output.CityDisplayProperty = string.Format("{0} {1}", _lookupCity.Code, _lookupCity.Name);
            }

            if (output.District.RegionId != null)
            {
                var _lookupRegion = await _lookup_regionRepository.FirstOrDefaultAsync((int)output.District.RegionId);
                output.RegionName = _lookupRegion?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Districts_Edit)]
        public async Task<GetDistrictForEditOutput> GetDistrictForEdit(EntityDto input)
        {
            var district = await _districtRepository.GetAllIncluding(x=>x.CityFk).FirstOrDefaultAsync(x=>x.Id==input.Id);

            var output = new GetDistrictForEditOutput { District = ObjectMapper.Map<CreateOrEditDistrictDto>(district) };

            if (output.District.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((int)output.District.CityId);
                output.CityDisplayProperty = string.Format("{0} {1}", _lookupCity.Code, _lookupCity.Name);
            }

            if (output.District.RegionId != null)
            {
                var _lookupRegion = await _lookup_regionRepository.FirstOrDefaultAsync((int)output.District.RegionId);
                output.RegionName = _lookupRegion?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDistrictDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Districts_Create)]
        protected virtual async Task Create(CreateOrEditDistrictDto input)
        {
            var district = ObjectMapper.Map<District>(input);

            await _districtRepository.InsertAsync(district);

        }

        [AbpAuthorize(AppPermissions.Pages_Districts_Edit)]
        protected virtual async Task Update(CreateOrEditDistrictDto input)
        {
            var district = await _districtRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, district);

        }

        [AbpAuthorize(AppPermissions.Pages_Districts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _districtRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDistrictsToExcel(GetAllDistrictsForExcelInput input)
        {

            var filteredDistricts = _districtRepository.GetAll()
                        .Include(e => e.CityFk)
                        .Include(e => e.RegionFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityDisplayPropertyFilter), e => string.Format("{0} {1}", e.CityFk == null || e.CityFk.Code == null ? "" : e.CityFk.Code.ToString()
, e.CityFk == null || e.CityFk.Name == null ? "" : e.CityFk.Name.ToString()
) == input.CityDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionNameFilter), e => e.RegionFk != null && e.RegionFk.Name == input.RegionNameFilter);

            var query = (from o in filteredDistricts
                         join o1 in _lookup_cityRepository.GetAll() on o.CityId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_regionRepository.GetAll() on o.RegionId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetDistrictForViewDto()
                         {
                             District = new DistrictDto
                             {
                                 Name = o.Name,
                                 Order = o.Order,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             },
                             CityDisplayProperty = string.Format("{0} {1}", s1 == null || s1.Code == null ? "" : s1.Code.ToString()
, s1 == null || s1.Name == null ? "" : s1.Name.ToString()
),
                             RegionName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var districtListDtos = await query.ToListAsync();

            return _districtsExcelExporter.ExportToFile(districtListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_Districts)]
        public async Task<GetRegionForViewDto> GetRegionFromDistrict(int districtId)
        {
            var district = (await _districtRepository.GetAllIncluding(x => x.RegionFk).FirstOrDefaultAsync(x => x.Id == districtId));

            return ObjectMapper.Map<GetRegionForViewDto>(district?.RegionFk);
        }

        [AbpAuthorize(AppPermissions.Pages_Districts)]
        public async Task<List<DistrictLookupTableDto>> GetAllDistrictForTableDropdown(int? cityId = null)
        {
            return await _districtRepository.GetAll()
                .WhereIf(cityId.HasValue, x => x.CityId == cityId.Value)
                .Select(district => new DistrictLookupTableDto
                {
                    Id = district.Id,
                    DisplayName = district == null || district.Name == null ? "" : district.Name.ToString()
                }).ToListAsync();
        }
    }
}