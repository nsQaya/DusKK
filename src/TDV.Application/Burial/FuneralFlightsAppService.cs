using TDV.Burial;
using TDV.Flight;
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
    [AbpAuthorize(AppPermissions.Pages_FuneralFlights)]
    public class FuneralFlightsAppService : TDVAppServiceBase, IFuneralFlightsAppService
    {
        private readonly IRepository<FuneralFlight> _funeralFlightRepository;
        private readonly IFuneralFlightsExcelExporter _funeralFlightsExcelExporter;
        private readonly IRepository<Funeral, int> _lookup_funeralRepository;
        private readonly IRepository<AirlineCompany, int> _lookup_airlineCompanyRepository;
        private readonly IRepository<Airport, int> _lookup_airportRepository;
        private readonly IAirportsAppService _airportsAppService;
        private readonly IAirlineCompaniesAppService _airlineAppService;

        public FuneralFlightsAppService(
            IRepository<FuneralFlight> funeralFlightRepository, 
            IFuneralFlightsExcelExporter funeralFlightsExcelExporter, 
            IRepository<Funeral, int> lookup_funeralRepository, 
            IRepository<AirlineCompany, int> lookup_airlineCompanyRepository, 
            IRepository<Airport, int> lookup_airportRepository,
            IAirportsAppService airportsAppService,
            IAirlineCompaniesAppService airlineAppService)
        {
            _funeralFlightRepository = funeralFlightRepository;
            _funeralFlightsExcelExporter = funeralFlightsExcelExporter;
            _lookup_funeralRepository = lookup_funeralRepository;
            _lookup_airlineCompanyRepository = lookup_airlineCompanyRepository;
            _lookup_airportRepository = lookup_airportRepository;
            _airportsAppService = airportsAppService;
            _airlineAppService= airlineAppService;
        }

        public async Task<PagedResultDto<GetFuneralFlightForViewDto>> GetAll(GetAllFuneralFlightsInput input)
        {

            var filteredFuneralFlights = _funeralFlightRepository.GetAll()
                        .Include(e => e.FuneralFk)
                        .Include(e => e.AirlineCompanyFk)
                        .Include(e => e.LiftOffAirportFk)
                        .Include(e => e.LangingAirportFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.No.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoFilter), e => e.No.Contains(input.NoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.MinLiftOffDateFilter != null, e => e.LiftOffDate >= input.MinLiftOffDateFilter)
                        .WhereIf(input.MaxLiftOffDateFilter != null, e => e.LiftOffDate <= input.MaxLiftOffDateFilter)
                        .WhereIf(input.MinLandingDateFilter != null, e => e.LandingDate >= input.MinLandingDateFilter)
                        .WhereIf(input.MaxLandingDateFilter != null, e => e.LandingDate <= input.MaxLandingDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralNameFilter), e => e.FuneralFk != null && e.FuneralFk.Name == input.FuneralNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AirlineCompanyCodeFilter), e => e.AirlineCompanyFk != null && e.AirlineCompanyFk.Code == input.AirlineCompanyCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AirportNameFilter), e => e.LiftOffAirportFk != null && e.LiftOffAirportFk.Name == input.AirportNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AirportName2Filter), e => e.LangingAirportFk != null && e.LangingAirportFk.Name == input.AirportName2Filter)
                        .WhereIf(input.FuneralIdFilter.HasValue, e => false || e.FuneralId == input.FuneralIdFilter.Value);

            var pagedAndFilteredFuneralFlights = filteredFuneralFlights
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredFuneralFlights.CountAsync();

            var dbList = await pagedAndFilteredFuneralFlights.ToListAsync();
            

            return new PagedResultDto<GetFuneralFlightForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFuneralFlightForViewDto>>(dbList)
            );

        }

        public async Task<GetFuneralFlightForViewDto> GetFuneralFlightForView(int id)
        {
            var funeralFlight = await _funeralFlightRepository.GetAsync(id);

            var output = new GetFuneralFlightForViewDto { FuneralFlight = ObjectMapper.Map<FuneralFlightDto>(funeralFlight) };

            if (output.FuneralFlight.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.FuneralFlight.FuneralId);
                output.FuneralName = _lookupFuneral?.Name?.ToString();
            }

            if (output.FuneralFlight.AirlineCompanyId != null)
            {
                var _lookupAirlineCompany = await _lookup_airlineCompanyRepository.FirstOrDefaultAsync((int)output.FuneralFlight.AirlineCompanyId);
                output.AirlineCompanyCode = _lookupAirlineCompany?.Code?.ToString();
            }

            if (output.FuneralFlight.LiftOffAirportId != null)
            {
                var _lookupAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.FuneralFlight.LiftOffAirportId);
                output.AirportName = _lookupAirport?.Name?.ToString();
            }

            if (output.FuneralFlight.LangingAirportId != null)
            {
                var _lookupAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.FuneralFlight.LangingAirportId);
                output.AirportName2 = _lookupAirport?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralFlights_Edit)]
        public async Task<GetFuneralFlightForEditOutput> GetFuneralFlightForEdit(EntityDto input)
        {
            var funeralFlight = await _funeralFlightRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFuneralFlightForEditOutput { FuneralFlight = ObjectMapper.Map<CreateOrEditFuneralFlightDto>(funeralFlight) };

            if (output.FuneralFlight.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.FuneralFlight.FuneralId);
                output.FuneralName = _lookupFuneral?.Name?.ToString();
            }

            if (output.FuneralFlight.AirlineCompanyId != null)
            {
                var _lookupAirlineCompany = await _lookup_airlineCompanyRepository.FirstOrDefaultAsync((int)output.FuneralFlight.AirlineCompanyId);
                output.AirlineCompanyCode = _lookupAirlineCompany?.Code?.ToString();
            }

            if (output.FuneralFlight.LiftOffAirportId != null)
            {
                var _lookupAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.FuneralFlight.LiftOffAirportId);
                output.AirportName = _lookupAirport?.Name?.ToString();
            }

            if (output.FuneralFlight.LangingAirportId != null)
            {
                var _lookupAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.FuneralFlight.LangingAirportId);
                output.AirportName2 = _lookupAirport?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralFlights_Edit)]
        public async Task<GetFuneralFlightForEditOutput> GetFuneralFlightForStep(int funeralId)
        {
            var funeralFlight = await _funeralFlightRepository.FirstOrDefaultAsync(x=>x.FuneralId==funeralId);

            var output = new GetFuneralFlightForEditOutput { FuneralFlight = ObjectMapper.Map<CreateOrEditFuneralFlightDto>(funeralFlight) };

            output.AirportList = await _airportsAppService.GetAllAirportForTableDropdown();
            output.AirlineCompanyList = await _airlineAppService.GetAllAirlineCompanyForTableDropdown();

            if (output.FuneralFlight?.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.FuneralFlight.FuneralId);
                output.FuneralName = _lookupFuneral?.Name?.ToString();
            }

            if (output.FuneralFlight?.AirlineCompanyId != null)
            {
                var _lookupAirlineCompany = await _lookup_airlineCompanyRepository.FirstOrDefaultAsync((int)output.FuneralFlight.AirlineCompanyId);
                output.AirlineCompanyCode = _lookupAirlineCompany?.Code?.ToString();
            }

            if (output.FuneralFlight?.LiftOffAirportId != null)
            {
                var _lookupAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.FuneralFlight.LiftOffAirportId);
                output.AirportName = _lookupAirport?.Name?.ToString();
            }

            if (output.FuneralFlight?.LangingAirportId != null)
            {
                var _lookupAirport = await _lookup_airportRepository.FirstOrDefaultAsync((int)output.FuneralFlight.LangingAirportId);
                output.AirportName2 = _lookupAirport?.Name?.ToString();
            }

            

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFuneralFlightDto input)
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

        [AbpAuthorize(AppPermissions.Pages_FuneralFlights_Create)]
        protected virtual async Task Create(CreateOrEditFuneralFlightDto input)
        {
            var funeralFlight = ObjectMapper.Map<FuneralFlight>(input);

            await _funeralFlightRepository.InsertAsync(funeralFlight);

        }
        [AbpAuthorize(AppPermissions.Pages_FuneralFlights_Create)]
        public async Task<int> CreateAndGetId(CreateOrEditFuneralFlightDto input)
        {
            var funeralFlight = ObjectMapper.Map<FuneralFlight>(input);

            return await _funeralFlightRepository.InsertAndGetIdAsync(funeralFlight);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralFlights_Edit)]
        protected virtual async Task Update(CreateOrEditFuneralFlightDto input)
        {
            var funeralFlight = await _funeralFlightRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, funeralFlight);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralFlights_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _funeralFlightRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFuneralFlightsToExcel(GetAllFuneralFlightsForExcelInput input)
        {

            var filteredFuneralFlights = _funeralFlightRepository.GetAll()
                        .Include(e => e.FuneralFk)
                        .Include(e => e.AirlineCompanyFk)
                        .Include(e => e.LiftOffAirportFk)
                        .Include(e => e.LangingAirportFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.No.Contains(input.Filter) || e.Code.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoFilter), e => e.No.Contains(input.NoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(input.MinLiftOffDateFilter != null, e => e.LiftOffDate >= input.MinLiftOffDateFilter)
                        .WhereIf(input.MaxLiftOffDateFilter != null, e => e.LiftOffDate <= input.MaxLiftOffDateFilter)
                        .WhereIf(input.MinLandingDateFilter != null, e => e.LandingDate >= input.MinLandingDateFilter)
                        .WhereIf(input.MaxLandingDateFilter != null, e => e.LandingDate <= input.MaxLandingDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralNameFilter), e => e.FuneralFk != null && e.FuneralFk.Name == input.FuneralNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AirlineCompanyCodeFilter), e => e.AirlineCompanyFk != null && e.AirlineCompanyFk.Code == input.AirlineCompanyCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AirportNameFilter), e => e.LiftOffAirportFk != null && e.LiftOffAirportFk.Name == input.AirportNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AirportName2Filter), e => e.LangingAirportFk != null && e.LangingAirportFk.Name == input.AirportName2Filter);

            var query = (from o in filteredFuneralFlights
                         join o1 in _lookup_funeralRepository.GetAll() on o.FuneralId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_airlineCompanyRepository.GetAll() on o.AirlineCompanyId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_airportRepository.GetAll() on o.LiftOffAirportId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_airportRepository.GetAll() on o.LangingAirportId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetFuneralFlightForViewDto()
                         {
                             FuneralFlight = new FuneralFlightDto
                             {
                                 No = o.No,
                                 Code = o.Code,
                                 LiftOffDate = o.LiftOffDate,
                                 LandingDate = o.LandingDate,
                                 Id = o.Id
                             },
                             FuneralName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             AirlineCompanyCode = s2 == null || s2.Code == null ? "" : s2.Code.ToString(),
                             AirportName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             AirportName2 = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var funeralFlightListDtos = await query.ToListAsync();

            return _funeralFlightsExcelExporter.ExportToFile(funeralFlightListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralFlights)]
        public async Task<PagedResultDto<FuneralFlightFuneralLookupTableDto>> GetAllFuneralForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_funeralRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var funeralList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FuneralFlightFuneralLookupTableDto>();
            foreach (var funeral in funeralList)
            {
                lookupTableDtoList.Add(new FuneralFlightFuneralLookupTableDto
                {
                    Id = funeral.Id,
                    DisplayName = funeral.Name?.ToString()
                });
            }

            return new PagedResultDto<FuneralFlightFuneralLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralFlights)]
        public async Task<PagedResultDto<AirlineCompanyLookupTableDto>> GetAllAirlineCompanyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_airlineCompanyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Code != null && e.Code.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var airlineCompanyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AirlineCompanyLookupTableDto>();
            foreach (var airlineCompany in airlineCompanyList)
            {
                lookupTableDtoList.Add(new AirlineCompanyLookupTableDto
                {
                    Id = airlineCompany.Id,
                    DisplayName = airlineCompany.Code?.ToString()
                });
            }

            return new PagedResultDto<AirlineCompanyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralFlights)]
        public async Task<PagedResultDto<AirportLookupTableDto>> GetAllAirportForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_airportRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var airportList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<AirportLookupTableDto>();
            foreach (var airport in airportList)
            {
                lookupTableDtoList.Add(new AirportLookupTableDto
                {
                    Id = airport.Id,
                    DisplayName = airport.Name?.ToString()
                });
            }

            return new PagedResultDto<AirportLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}