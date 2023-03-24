using TDV.Flight;
using TDV.Location;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Flight.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;

namespace TDV.Flight
{
    [AbpAuthorize(AppPermissions.Pages_AirportRegions)]
    public class AirportRegionsAppService : TDVAppServiceBase, IAirportRegionsAppService
    {
        private readonly IRepository<AirportRegion> _airportRegionRepository;
        private readonly IRepository<Airport, int> _lookup_airportRepository;
        private readonly IRepository<Region, int> _lookup_regionRepository;

        public AirportRegionsAppService(IRepository<AirportRegion> airportRegionRepository, IRepository<Airport, int> lookup_airportRepository, IRepository<Region, int> lookup_regionRepository)
        {
            _airportRegionRepository = airportRegionRepository;
            _lookup_airportRepository = lookup_airportRepository;
            _lookup_regionRepository = lookup_regionRepository;

        }

        public async Task<PagedResultDto<GetAirportRegionForViewDto>> GetAll(GetAllAirportRegionsInput input)
        {

            var filteredAirportRegions = _airportRegionRepository.GetAll()
                        .Include(e => e.AirportFk)
                        .Include(e => e.RegionFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AirportDisplayPropertyFilter), e => string.Format("{0} {1}", e.AirportFk == null || e.AirportFk.Code == null ? "" : e.AirportFk.Code.ToString()
, e.AirportFk == null || e.AirportFk.Name == null ? "" : e.AirportFk.Name.ToString()
) == input.AirportDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionNameFilter), e => e.RegionFk != null && e.RegionFk.Name == input.RegionNameFilter);

            var pagedAndFilteredAirportRegions = filteredAirportRegions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var airportRegions = from o in pagedAndFilteredAirportRegions
                                 join o1 in _lookup_airportRepository.GetAll() on o.AirportId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 join o2 in _lookup_regionRepository.GetAll() on o.RegionId equals o2.Id into j2
                                 from s2 in j2.DefaultIfEmpty()

                                 select new
                                 {

                                     Id = o.Id,
                                     AirportDisplayProperty = string.Format("{0} {1}", s1 == null || s1.Code == null ? "" : s1.Code.ToString()
                 , s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                 ),
                                     RegionName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                 };

            var totalCount = await filteredAirportRegions.CountAsync();

            var dbList = await airportRegions.ToListAsync();
            var results = new List<GetAirportRegionForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetAirportRegionForViewDto()
                {
                    AirportRegion = new AirportRegionDto
                    {

                        Id = o.Id,
                    },
                    AirportDisplayProperty = o.AirportDisplayProperty,
                    RegionName = o.RegionName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetAirportRegionForViewDto>(
                totalCount,
                results
            );

        }
        [AbpAuthorize(AppPermissions.Pages_AirportRegions_Edit)]
        public async Task<GetAirportRegionForEditOutput> GetAirportRegionForEdit(EntityDto input)
        {
            var airportRegion = await _airportRegionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAirportRegionForEditOutput { AirportRegion = ObjectMapper.Map<CreateOrEditAirportRegionDto>(airportRegion) };

            if (output.AirportRegion?.AirportId != null)
            {
                var _lookupAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.AirportRegion.AirportId);
                output.AirportDisplayProperty = string.Format("{0} {1}", _lookupAirport.Code, _lookupAirport.Name);
            }

            if (output.AirportRegion?.RegionId != null)
            {
                var _lookupRegion = await _lookup_regionRepository.FirstOrDefaultAsync((int)output.AirportRegion.RegionId);
                output.RegionName = _lookupRegion?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAirportRegionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AirportRegions_Create)]
        protected virtual async Task Create(CreateOrEditAirportRegionDto input)
        {
            var airportRegion = ObjectMapper.Map<AirportRegion>(input);

            await _airportRegionRepository.InsertAsync(airportRegion);

        }

        [AbpAuthorize(AppPermissions.Pages_AirportRegions_Edit)]
        protected virtual async Task Update(CreateOrEditAirportRegionDto input)
        {
            var airportRegion = await _airportRegionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, airportRegion);

        }

        [AbpAuthorize(AppPermissions.Pages_AirportRegions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _airportRegionRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_AirportRegions)]
        public async Task<PagedResultDto<AirportRegionAirportLookupTableDto>> GetAllAirportForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_airportRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => string.Format("{0} {1}", e.Code, e.Name).Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var airportList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AirportRegionAirportLookupTableDto>();
            foreach (var airport in airportList)
            {
                lookupTableDtoList.Add(new AirportRegionAirportLookupTableDto
                {
                    Id = airport.Id,
                    DisplayName = string.Format("{0} {1}", airport.Code, airport.Name)
                });
            }

            return new PagedResultDto<AirportRegionAirportLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_AirportRegions)]
        public async Task<PagedResultDto<AirportRegionRegionLookupTableDto>> GetAllRegionForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_regionRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var regionList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AirportRegionRegionLookupTableDto>();
            foreach (var region in regionList)
            {
                lookupTableDtoList.Add(new AirportRegionRegionLookupTableDto
                {
                    Id = region.Id,
                    DisplayName = region.Name?.ToString()
                });
            }

            return new PagedResultDto<AirportRegionRegionLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}