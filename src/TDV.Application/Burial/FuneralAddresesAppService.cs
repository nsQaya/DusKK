using TDV.Burial;
using TDV.Location;

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
    [AbpAuthorize(AppPermissions.Pages_FuneralAddreses)]
    public class FuneralAddresesAppService : TDVAppServiceBase, IFuneralAddresesAppService
    {
        private readonly ICountriesAppService _countriesAppService;
        private readonly ICitiesAppService _citiesAppService;
        private readonly IDistrictsAppService _districtsAppService;
        private readonly IQuartersAppService _quartersAppService;

        private readonly IRepository<FuneralAddres> _funeralAddresRepository;
        private readonly IFuneralAddresesExcelExporter _funeralAddresesExcelExporter;
        private readonly IRepository<Funeral, int> _lookup_funeralRepository;
        private readonly IRepository<Quarter, int> _lookup_quarterRepository;

        public FuneralAddresesAppService(
            IRepository<FuneralAddres> funeralAddresRepository, 
            IFuneralAddresesExcelExporter funeralAddresesExcelExporter, 
            IRepository<Funeral, int> lookup_funeralRepository, 
            IRepository<Quarter, int> lookup_quarterRepository,
            ICountriesAppService countriesAppService,
            ICitiesAppService citiesAppService, 
            IDistrictsAppService districtsAppService,
            IQuartersAppService quartersAppService
            )
        {
            _funeralAddresRepository = funeralAddresRepository;
            _funeralAddresesExcelExporter = funeralAddresesExcelExporter;
            _lookup_funeralRepository = lookup_funeralRepository;
            _lookup_quarterRepository = lookup_quarterRepository;
            _countriesAppService = countriesAppService;
            _citiesAppService = citiesAppService;
            _districtsAppService = districtsAppService;
            _quartersAppService = quartersAppService;
        }

        public async Task<PagedResultDto<GetFuneralAddresForViewDto>> GetAll(GetAllFuneralAddresesInput input)
        {

            var filteredFuneralAddreses = _funeralAddresRepository.GetAll()
                        .Include(e => e.FuneralFk)
                        .Include(e => e.QuarterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Address.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.FuneralFk == null || e.FuneralFk.TransferNo == null ? "" : e.FuneralFk.TransferNo.ToString()
, e.FuneralFk == null || e.FuneralFk.Name == null ? "" : e.FuneralFk.Name.ToString()
, e.FuneralFk == null || e.FuneralFk.Surname == null ? "" : e.FuneralFk.Surname.ToString()
) == input.FuneralDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuarterNameFilter), e => e.QuarterFk != null && e.QuarterFk.Name == input.QuarterNameFilter);

            var pagedAndFilteredFuneralAddreses = filteredFuneralAddreses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredFuneralAddreses.CountAsync();

            var dbList = await pagedAndFilteredFuneralAddreses.ToListAsync();
            return new PagedResultDto<GetFuneralAddresForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFuneralAddresForViewDto>>(dbList)
            );

        }

        public async Task<GetFuneralAddresForViewDto> GetFuneralAddresForView(int id)
        {
            var funeralAddres = await _funeralAddresRepository.GetAsync(id);

            var output = new GetFuneralAddresForViewDto { FuneralAddres = ObjectMapper.Map<FuneralAddresDto>(funeralAddres) };

            if (output.FuneralAddres.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.FuneralAddres.FuneralId);
                output.FuneralDisplayProperty = string.Format("{0} {1} {2}", _lookupFuneral.TransferNo, _lookupFuneral.Name, _lookupFuneral.Surname);
            }

            if (output.FuneralAddres.QuarterId != null)
            {
                var _lookupQuarter = await _lookup_quarterRepository.FirstOrDefaultAsync((int)output.FuneralAddres.QuarterId);
                output.QuarterName = _lookupQuarter?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralAddreses_Edit)]
        public async Task<GetFuneralAddresForEditOutput> GetFuneralAddresForEdit(EntityDto input)
        {
            var funeralAddres = await _funeralAddresRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFuneralAddresForEditOutput { FuneralAddres = ObjectMapper.Map<CreateOrEditFuneralAddresDto>(funeralAddres) };

            if (output.FuneralAddres.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.FuneralAddres.FuneralId);
                output.FuneralDisplayProperty = string.Format("{0} {1} {2}", _lookupFuneral.TransferNo, _lookupFuneral.Name, _lookupFuneral.Surname);
            }

            if (output.FuneralAddres.QuarterId != null)
            {
                var _lookupQuarter = await _lookup_quarterRepository.FirstOrDefaultAsync((int)output.FuneralAddres.QuarterId);
                output.QuarterName = _lookupQuarter?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralAddreses_Edit)]
        public async Task<GetFuneralAddresForEditOutput> GetFuneralAddresForStep(int funeralId)
        {
            var funeralAddres = await _funeralAddresRepository.FirstOrDefaultAsync(x=>x.FuneralId==funeralId);

           

            var output = new GetFuneralAddresForEditOutput { FuneralAddres = ObjectMapper.Map<CreateOrEditFuneralAddresDto>(funeralAddres) };

            output.CountryList = await _countriesAppService.GetAllCountryForTableDropdown();

            if (output.FuneralAddres?.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.FuneralAddres.FuneralId);
                output.FuneralDisplayProperty = string.Format("{0} {1} {2}", _lookupFuneral.TransferNo, _lookupFuneral.Name, _lookupFuneral.Surname);
            }

            if (output.FuneralAddres?.QuarterId != null)
            {
                var _lookupQuarter = await _lookup_quarterRepository
                    .GetAllIncluding()
                    .Include(x => x.DistrictFk)
                        .ThenInclude(x => x.CityFk)
                            .ThenInclude(x => x.CountryFk)
                    .Include(x => x.DistrictFk)
                        .ThenInclude(x => x.RegionFk)
                    .FirstOrDefaultAsync(x=> x.Id==output.FuneralAddres.QuarterId);

                output.RegionName = _lookupQuarter.DistrictFk.RegionFk.Name;
                output.QuarterName = _lookupQuarter.Name;

                output.CountryId = _lookupQuarter.DistrictFk.CityFk.CountryId;
                output.CityList = await _citiesAppService.GetAllCityForTableDropdown(_lookupQuarter.DistrictFk.CityFk.CountryId);
                output.CityId = _lookupQuarter.DistrictFk.CityId;
                output.DistrictList = await _districtsAppService.GetAllDistrictForTableDropdown(_lookupQuarter.DistrictFk.CityId);
                output.DistrictId = _lookupQuarter.Id;
                output.QuarterList = await _quartersAppService.GetAllQuartersForTableDropdown(_lookupQuarter.Id);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFuneralAddresDto input)
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

        [AbpAuthorize(AppPermissions.Pages_FuneralAddreses_Create)]
        protected virtual async Task Create(CreateOrEditFuneralAddresDto input)
        {
            var funeralAddres = ObjectMapper.Map<FuneralAddres>(input);

            await _funeralAddresRepository.InsertAsync(funeralAddres);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralAddreses_Create)]
        public async Task<int> CreateAndGetId(CreateOrEditFuneralAddresDto input)
        {
            var funeralAddres = ObjectMapper.Map<FuneralAddres>(input);
            return await _funeralAddresRepository.InsertAndGetIdAsync(funeralAddres);
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralAddreses_Edit)]
        protected virtual async Task Update(CreateOrEditFuneralAddresDto input)
        {
            var funeralAddres = await _funeralAddresRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, funeralAddres);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralAddreses_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _funeralAddresRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFuneralAddresesToExcel(GetAllFuneralAddresesForExcelInput input)
        {

            var filteredFuneralAddreses = _funeralAddresRepository.GetAll()
                        .Include(e => e.FuneralFk)
                        .Include(e => e.QuarterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Description.Contains(input.Filter) || e.Address.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.FuneralFk == null || e.FuneralFk.TransferNo == null ? "" : e.FuneralFk.TransferNo.ToString()
, e.FuneralFk == null || e.FuneralFk.Name == null ? "" : e.FuneralFk.Name.ToString()
, e.FuneralFk == null || e.FuneralFk.Surname == null ? "" : e.FuneralFk.Surname.ToString()
) == input.FuneralDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuarterNameFilter), e => e.QuarterFk != null && e.QuarterFk.Name == input.QuarterNameFilter);

            var query = (from o in filteredFuneralAddreses
                         join o1 in _lookup_funeralRepository.GetAll() on o.FuneralId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_quarterRepository.GetAll() on o.QuarterId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetFuneralAddresForViewDto()
                         {
                             FuneralAddres = new FuneralAddresDto
                             {
                                 Description = o.Description,
                                 Address = o.Address,
                                 Id = o.Id
                             },
                             FuneralDisplayProperty = string.Format("{0} {1} {2}", s1 == null || s1.TransferNo == null ? "" : s1.TransferNo.ToString()
, s1 == null || s1.Name == null ? "" : s1.Name.ToString()
, s1 == null || s1.Surname == null ? "" : s1.Surname.ToString()
),
                             QuarterName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var funeralAddresListDtos = await query.ToListAsync();

            return _funeralAddresesExcelExporter.ExportToFile(funeralAddresListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralAddreses)]
        public async Task<PagedResultDto<FuneralAddresFuneralLookupTableDto>> GetAllFuneralForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_funeralRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => string.Format("{0} {1} {2}", e.TransferNo, e.Name, e.Surname).Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var funeralList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FuneralAddresFuneralLookupTableDto>();
            foreach (var funeral in funeralList)
            {
                lookupTableDtoList.Add(new FuneralAddresFuneralLookupTableDto
                {
                    Id = funeral.Id,
                    DisplayName = string.Format("{0} {1} {2}", funeral.TransferNo, funeral.Name, funeral.Surname)
                });
            }

            return new PagedResultDto<FuneralAddresFuneralLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralAddreses)]
        public async Task<PagedResultDto<FuneralAddresQuarterLookupTableDto>> GetAllQuarterForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_quarterRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var quarterList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FuneralAddresQuarterLookupTableDto>();
            foreach (var quarter in quarterList)
            {
                lookupTableDtoList.Add(new FuneralAddresQuarterLookupTableDto
                {
                    Id = quarter.Id,
                    DisplayName = quarter.Name?.ToString()
                });
            }

            return new PagedResultDto<FuneralAddresQuarterLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}