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
    [AbpAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesAppService : TDVAppServiceBase, ICountriesAppService
    {
        private readonly IRepository<Country> _countryRepository;
        private readonly ICountriesExcelExporter _countriesExcelExporter;

        public CountriesAppService(IRepository<Country> countryRepository, ICountriesExcelExporter countriesExcelExporter)
        {
            _countryRepository = countryRepository;
            _countriesExcelExporter = countriesExcelExporter;

        }

        public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input)
        {

            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredCountries = filteredCountries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredCountries.CountAsync();

            var dbList = await pagedAndFilteredCountries.ToListAsync();
           
            return new PagedResultDto<GetCountryForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetCountryForViewDto>>(dbList)
            );

        }

        public async Task<GetCountryForViewDto> GetCountryForView(int id)
        {
            var country = await _countryRepository.GetAsync(id);

            var output = new GetCountryForViewDto { Country = ObjectMapper.Map<CountryDto>(country) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCountryForEditOutput { Country = ObjectMapper.Map<CreateOrEditCountryDto>(country) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCountryDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Countries_Create)]
        protected virtual async Task Create(CreateOrEditCountryDto input)
        {
            var country = ObjectMapper.Map<Country>(input);

            await _countryRepository.InsertAsync(country);

        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        protected virtual async Task Update(CreateOrEditCountryDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, country);

        }

        [AbpAuthorize(AppPermissions.Pages_Countries_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _countryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input)
        {

            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(input.MinOrderFilter != null, e => e.Order >= input.MinOrderFilter)
                        .WhereIf(input.MaxOrderFilter != null, e => e.Order <= input.MaxOrderFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredCountries
                         select new GetCountryForViewDto()
                         {
                             Country = new CountryDto
                             {
                                 Code = o.Code,
                                 Name = o.Name,
                                 Order = o.Order,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var countryListDtos = await query.ToListAsync();

            return _countriesExcelExporter.ExportToFile(countryListDtos);
        }
        [AbpAuthorize(AppPermissions.Pages_Countries)]
        public async Task<List<CountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _countryRepository.GetAll()
                .Select(country => new CountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = string.Format("{0} {1}", country.Code, country.Name)
                }).ToListAsync();
        }
    }
}