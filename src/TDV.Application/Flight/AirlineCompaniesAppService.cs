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
    [AbpAuthorize(AppPermissions.Pages_AirlineCompanies)]
    public class AirlineCompaniesAppService : TDVAppServiceBase, IAirlineCompaniesAppService
    {
        private readonly IRepository<AirlineCompany> _airlineCompanyRepository;
        private readonly IAirlineCompaniesExcelExporter _airlineCompaniesExcelExporter;

        public AirlineCompaniesAppService(IRepository<AirlineCompany> airlineCompanyRepository, IAirlineCompaniesExcelExporter airlineCompaniesExcelExporter)
        {
            _airlineCompanyRepository = airlineCompanyRepository;
            _airlineCompaniesExcelExporter = airlineCompaniesExcelExporter;

        }

        public async Task<PagedResultDto<GetAirlineCompanyForViewDto>> GetAll(GetAllAirlineCompaniesInput input)
        {

            var filteredAirlineCompanies = _airlineCompanyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.LadingPrefix.Contains(input.Filter) || e.FlightPrefix.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LadingPrefixFilter), e => e.LadingPrefix.Contains(input.LadingPrefixFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FlightPrefixFilter), e => e.FlightPrefix.Contains(input.FlightPrefixFilter));

            var pagedAndFilteredAirlineCompanies = filteredAirlineCompanies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredAirlineCompanies.CountAsync();

            var dbList = await pagedAndFilteredAirlineCompanies.ToListAsync();
           
            return new PagedResultDto<GetAirlineCompanyForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetAirlineCompanyForViewDto>>(dbList)
            );

        }

        public async Task<GetAirlineCompanyForViewDto> GetAirlineCompanyForView(int id)
        {
            var airlineCompany = await _airlineCompanyRepository.GetAsync(id);

            var output = new GetAirlineCompanyForViewDto { AirlineCompany = ObjectMapper.Map<AirlineCompanyDto>(airlineCompany) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_AirlineCompanies_Edit)]
        public async Task<GetAirlineCompanyForEditOutput> GetAirlineCompanyForEdit(EntityDto input)
        {
            var airlineCompany = await _airlineCompanyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetAirlineCompanyForEditOutput { AirlineCompany = ObjectMapper.Map<CreateOrEditAirlineCompanyDto>(airlineCompany) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditAirlineCompanyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_AirlineCompanies_Create)]
        protected virtual async Task Create(CreateOrEditAirlineCompanyDto input)
        {
            var airlineCompany = ObjectMapper.Map<AirlineCompany>(input);

            await _airlineCompanyRepository.InsertAsync(airlineCompany);

        }

        [AbpAuthorize(AppPermissions.Pages_AirlineCompanies_Edit)]
        protected virtual async Task Update(CreateOrEditAirlineCompanyDto input)
        {
            var airlineCompany = await _airlineCompanyRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, airlineCompany);

        }

        [AbpAuthorize(AppPermissions.Pages_AirlineCompanies_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _airlineCompanyRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetAirlineCompaniesToExcel(GetAllAirlineCompaniesForExcelInput input)
        {

            var filteredAirlineCompanies = _airlineCompanyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.LadingPrefix.Contains(input.Filter) || e.FlightPrefix.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LadingPrefixFilter), e => e.LadingPrefix.Contains(input.LadingPrefixFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FlightPrefixFilter), e => e.FlightPrefix.Contains(input.FlightPrefixFilter));

            var query = (from o in filteredAirlineCompanies
                         select new GetAirlineCompanyForViewDto()
                         {
                             AirlineCompany = new AirlineCompanyDto
                             {
                                 Code = o.Code,
                                 Name = o.Name,
                                 LadingPrefix = o.LadingPrefix,
                                 FlightPrefix = o.FlightPrefix,
                                 Id = o.Id
                             }
                         });

            var airlineCompanyListDtos = await query.ToListAsync();

            return _airlineCompaniesExcelExporter.ExportToFile(airlineCompanyListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_AirlineCompanies)]
        public async Task<List<AirlineCompanyLookupTableDto>> GetAllAirlineCompanyForTableDropdown()
        {
            return await _airlineCompanyRepository.GetAll()
                .Select(company => new AirlineCompanyLookupTableDto
                {
                    Id = company.Id,
                    DisplayName = string.Format("{0} {1}", company.Code, company.Name)
                }).ToListAsync();
        }

    }
}