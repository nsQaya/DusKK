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
    [AbpAuthorize(AppPermissions.Pages_Cities)]
    public class CitiesAppService : TDVAppServiceBase, ICitiesAppService
    {
        private readonly IRepository<City> _cityRepository;
        private readonly ICitiesExcelExporter _citiesExcelExporter;
        private readonly IRepository<Country, int> _lookup_countryRepository;

        public CitiesAppService(IRepository<City> cityRepository, ICitiesExcelExporter citiesExcelExporter, IRepository<Country, int> lookup_countryRepository)
        {
            _cityRepository = cityRepository;
            _citiesExcelExporter = citiesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;

        }

        public async Task<PagedResultDto<GetCityForViewDto>> GetAll(GetAllCitiesInput input)
        {

            var filteredCities = _cityRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryDisplayPropertyFilter), e => string.Format("{0} {1}", e.CountryFk == null || e.CountryFk.Code == null ? "" : e.CountryFk.Code.ToString()
, e.CountryFk == null || e.CountryFk.Name == null ? "" : e.CountryFk.Name.ToString()
) == input.CountryDisplayPropertyFilter);

            var pagedAndFilteredCities = filteredCities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);
         
            var totalCount = await filteredCities.CountAsync();

            var dbList = await pagedAndFilteredCities.ToListAsync();
          
            return new PagedResultDto<GetCityForViewDto>(
                totalCount,
                 ObjectMapper.Map<List<GetCityForViewDto>>(dbList)
            );

        }

        public async Task<GetCityForViewDto> GetCityForView(int id)
        {
            var city = await _cityRepository.GetAsync(id);

            var output = new GetCityForViewDto { City = ObjectMapper.Map<CityDto>(city) };

            if (output.City.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((int)output.City.CountryId);
                output.CountryDisplayProperty = string.Format("{0} {1}", _lookupCountry.Code, _lookupCountry.Name);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Cities_Edit)]
        public async Task<GetCityForEditOutput> GetCityForEdit(EntityDto input)
        {
            var city = await _cityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCityForEditOutput { City = ObjectMapper.Map<CreateOrEditCityDto>(city) };

            if (output.City.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((int)output.City.CountryId);
                output.CountryDisplayProperty = string.Format("{0} {1}", _lookupCountry.Code, _lookupCountry.Name);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCityDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Cities_Create)]
        protected virtual async Task Create(CreateOrEditCityDto input)
        {
            var city = ObjectMapper.Map<City>(input);

            await _cityRepository.InsertAsync(city);

        }

        [AbpAuthorize(AppPermissions.Pages_Cities_Edit)]
        protected virtual async Task Update(CreateOrEditCityDto input)
        {
            var city = await _cityRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, city);

        }

        [AbpAuthorize(AppPermissions.Pages_Cities_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _cityRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input)
        {

            var filteredCities = _cityRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryDisplayPropertyFilter), e => string.Format("{0} {1}", e.CountryFk == null || e.CountryFk.Code == null ? "" : e.CountryFk.Code.ToString()
, e.CountryFk == null || e.CountryFk.Name == null ? "" : e.CountryFk.Name.ToString()
) == input.CountryDisplayPropertyFilter);

            var query = (from o in filteredCities
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetCityForViewDto()
                         {
                             City = new CityDto
                             {
                                 Code = o.Code,
                                 Name = o.Name,
                                 Order = o.Order,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             },
                             CountryDisplayProperty = string.Format("{0} {1}", s1 == null || s1.Code == null ? "" : s1.Code.ToString()
, s1 == null || s1.Name == null ? "" : s1.Name.ToString()
)
                         });

            var cityListDtos = await query.ToListAsync();

            return _citiesExcelExporter.ExportToFile(cityListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Cities)]
        public async Task<List<CityLookupTableDto>> GetAllCityForTableDropdown(int? countryId = null)
        {
            return await _cityRepository.GetAll()
                .WhereIf(countryId.HasValue, x => x.CountryId == countryId.Value)
                .Select(city => new CityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = string.Format("{0} {1}", city.Code, city.Name)
                }).ToListAsync();
        }

    }
}