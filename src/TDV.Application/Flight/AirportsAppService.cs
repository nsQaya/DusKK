using TDV.Location;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Flight.Exporting;
using TDV.Flight.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;
using TDV.Burial.Dtos;

namespace TDV.Flight
{
    [AbpAuthorize(AppPermissions.Pages_Airports)]
    public class AirportsAppService : TDVAppServiceBase, IAirportsAppService
    {
        private readonly IRepository<Airport> _airportRepository;
        private readonly IAirportsExcelExporter _airportsExcelExporter;
        private readonly IRepository<Country, int> _lookup_countryRepository;
        private readonly IRepository<City, int> _lookup_cityRepository;

        public AirportsAppService(IRepository<Airport> airportRepository, IAirportsExcelExporter airportsExcelExporter, IRepository<Country, int> lookup_countryRepository, IRepository<City, int> lookup_cityRepository)
        {
            _airportRepository = airportRepository;
            _airportsExcelExporter = airportsExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_cityRepository = lookup_cityRepository;

        }

        public async Task<PagedResultDto<GetAirportForViewDto>> GetAll(GetAllAirportsInput input)
        {

            var filteredAirports = _airportRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.CityFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryDisplayPropertyFilter), e => string.Format("{0} {1}", e.CountryFk == null || e.CountryFk.Code == null ? "" : e.CountryFk.Code.ToString()
, e.CountryFk == null || e.CountryFk.Name == null ? "" : e.CountryFk.Name.ToString()
) == input.CountryDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityDisplayPropertyFilter), e => string.Format("{0} {1}", e.CityFk == null || e.CityFk.Code == null ? "" : e.CityFk.Code.ToString()
, e.CityFk == null || e.CityFk.Name == null ? "" : e.CityFk.Name.ToString()
) == input.CityDisplayPropertyFilter);

            var pagedAndFilteredAirports = filteredAirports
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredAirports.CountAsync();

            var dbList = await pagedAndFilteredAirports.ToListAsync();
            
            return new PagedResultDto<GetAirportForViewDto>(
                totalCount,
                 ObjectMapper.Map<List<GetAirportForViewDto>>(dbList)
            );

        }

        public async Task<GetAirportForViewDto> GetAirportForView(int id)
        {
            var airport = await _airportRepository.GetAsync(id);

            var output = new GetAirportForViewDto { Airport = ObjectMapper.Map<AirportDto>(airport) };

            if (output.Airport.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((int)output.Airport.CountryId);
                output.CountryDisplayProperty = string.Format("{0} {1}", _lookupCountry.Code, _lookupCountry.Name);
            }

            if (output.Airport.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((int)output.Airport.CityId);
                output.CityDisplayProperty = string.Format("{0} {1}", _lookupCity.Code, _lookupCity.Name);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Airports_Edit)]
        public async Task<GetAirportForEditOutput> GetAirportForEdit(EntityDto input)
        {
            var airport = await _airportRepository.GetAllIncluding(x=>x.Regions).FirstOrDefaultAsync(x=>x.Id==input.Id);

            var output = new GetAirportForEditOutput { Airport = ObjectMapper.Map<CreateOrEditAirportDto>(airport) };

            if (output.Airport.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((int)output.Airport.CountryId);
                output.CountryDisplayProperty = string.Format("{0} {1}", _lookupCountry.Code, _lookupCountry.Name);
            }

            if (output.Airport.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((int)output.Airport.CityId);
                output.CityDisplayProperty = string.Format("{0} {1}", _lookupCity.Code, _lookupCity.Name);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAirportDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Airports_Create)]
        protected virtual async Task Create(CreateOrEditAirportDto input)
        {
            var airport = ObjectMapper.Map<Airport>(input);

            await _airportRepository.InsertAsync(airport);

        }

        [AbpAuthorize(AppPermissions.Pages_Airports_Edit)]
        protected virtual async Task Update(CreateOrEditAirportDto input)
        {
            var airport = await _airportRepository.GetAllIncluding(x => x.Regions).FirstOrDefaultAsync(x => x.Id == (int)input.Id);
            airport.Regions?.Clear();
            ObjectMapper.Map(input, airport);

        }

        [AbpAuthorize(AppPermissions.Pages_Airports_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _airportRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAirportsToExcel(GetAllAirportsForExcelInput input)
        {

            var filteredAirports = _airportRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.CityFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryDisplayPropertyFilter), e => string.Format("{0} {1}", e.CountryFk == null || e.CountryFk.Code == null ? "" : e.CountryFk.Code.ToString()
, e.CountryFk == null || e.CountryFk.Name == null ? "" : e.CountryFk.Name.ToString()
) == input.CountryDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityDisplayPropertyFilter), e => string.Format("{0} {1}", e.CityFk == null || e.CityFk.Code == null ? "" : e.CityFk.Code.ToString()
, e.CityFk == null || e.CityFk.Name == null ? "" : e.CityFk.Name.ToString()
) == input.CityDisplayPropertyFilter);

            var query = (from o in filteredAirports
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_cityRepository.GetAll() on o.CityId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetAirportForViewDto()
                         {
                             Airport = new AirportDto
                             {
                                 Code = o.Code,
                                 Name = o.Name,
                                 Description = o.Description,
                                 Order = o.Order,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             },
                             CountryDisplayProperty = string.Format("{0} {1}", s1 == null || s1.Code == null ? "" : s1.Code.ToString()
, s1 == null || s1.Name == null ? "" : s1.Name.ToString()
),
                             CityDisplayProperty = string.Format("{0} {1}", s2 == null || s2.Code == null ? "" : s2.Code.ToString()
, s2 == null || s2.Name == null ? "" : s2.Name.ToString()
)
                         });

            var airportListDtos = await query.ToListAsync();

            return _airportsExcelExporter.ExportToFile(airportListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Airports)]
        public async Task<List<AirportLookupTableDto>> GetAllAirportForTableDropdown(int? cityId = null)
        {
            return await _airportRepository.GetAll()
                .WhereIf(cityId.HasValue,x=>x.CityId==cityId)
                .Select(airport => new AirportLookupTableDto
                {
                    Id = airport.Id,
                    DisplayName = string.Format("{0} {1}", airport.Code, airport.Name)
                }).ToListAsync();
        }

    }
}