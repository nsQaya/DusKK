using Abp.Organizations;
using TDV.Location;

using TDV.Corporation;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Corporation.Exporting;
using TDV.Corporation.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;
using System.Globalization;
using TDV.Organizations;
using TDV.Organizations.Dto;

namespace TDV.Corporation
{
    [AbpAuthorize(AppPermissions.Pages_Companies)]
    public class CompaniesAppService : TDVAppServiceBase, ICompaniesAppService
    {
        private readonly IRepository<Company> _companyRepository;
        private readonly ICompaniesExcelExporter _companiesExcelExporter;
        private readonly IRepository<OrganizationUnit, long> _lookup_organizationUnitRepository;
        private readonly IRepository<City, int> _lookup_cityRepository;
        private readonly IRepository<Quarter, int> _lookup_quarterRepository;
        private readonly OrganizationUnitAppService _organizationUnitAppService;

        public CompaniesAppService(IRepository<Company> companyRepository, 
            ICompaniesExcelExporter companiesExcelExporter, 
            IRepository<OrganizationUnit, long> lookup_organizationUnitRepository, 
            IRepository<City, int> lookup_cityRepository, 
            IRepository<Quarter, int> lookup_quarterRepository,
            OrganizationUnitAppService organizationUnitAppService
            )
        {
            _companyRepository = companyRepository;
            _companiesExcelExporter = companiesExcelExporter;
            _lookup_organizationUnitRepository = lookup_organizationUnitRepository;
            _lookup_cityRepository = lookup_cityRepository;
            _lookup_quarterRepository = lookup_quarterRepository;
            _organizationUnitAppService = organizationUnitAppService;

        }

        public async Task<PagedResultDto<GetCompanyForViewDto>> GetAll(GetAllCompaniesInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                        ? (CompanyType)input.TypeFilter
                        : default;

            var filteredCompanies = _companyRepository.GetAll()
                        .Include(e => e.OrganizationUnitFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.QuarterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TaxAdministration.Contains(input.Filter) || e.TaxNo.Contains(input.Filter) || e.Website.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Fax.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.RunningCode.Contains(input.Filter) || e.Prefix.Contains(input.Filter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAdministrationFilter), e => e.TaxAdministration.Contains(input.TaxAdministrationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxNoFilter), e => e.TaxNo.Contains(input.TaxNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteFilter), e => e.Website.Contains(input.WebsiteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FaxFilter), e => e.Fax.Contains(input.FaxFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RunningCodeFilter), e => e.RunningCode.Contains(input.RunningCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrefixFilter), e => e.Prefix.Contains(input.PrefixFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OwnerOrganizationUnitDisplayName), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OwnerOrganizationUnitDisplayName)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityDisplayPropertyFilter), e => string.Format("{0} {1}", e.CityFk == null || e.CityFk.Code == null ? "" : e.CityFk.Code.ToString()
, e.CityFk == null || e.CityFk.Name == null ? "" : e.CityFk.Name.ToString()
) == input.CityDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuarterNameFilter), e => e.QuarterFk != null && e.QuarterFk.Name == input.QuarterNameFilter);

            var pagedAndFilteredCompanies = filteredCompanies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

    

            var totalCount = await filteredCompanies.CountAsync();

            var dbList = await pagedAndFilteredCompanies.ToListAsync();
            var results = new List<GetCompanyForViewDto>();

            return new PagedResultDto<GetCompanyForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetCompanyForViewDto>>(dbList)
            );

        }

        public async Task<GetCompanyForViewDto> GetCompanyForView(int id)
        {
            var company = await _companyRepository.GetAsync(id);

            var output = new GetCompanyForViewDto { Company = ObjectMapper.Map<CompanyDto>(company) };

            if (output.Company.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Company.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            if (output.Company.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((int)output.Company.CityId);
                output.CityDisplayProperty = string.Format("{0} {1}", _lookupCity.Code, _lookupCity.Name);
            }

            if (output.Company.QuarterId != null)
            {
                var _lookupQuarter = await _lookup_quarterRepository.FirstOrDefaultAsync((int)output.Company.QuarterId);
                output.QuarterName = _lookupQuarter?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Companies_Edit)]
        public async Task<GetCompanyForEditOutput> GetCompanyForEdit(EntityDto input)
        {
            var company = await _companyRepository.GetAllIncluding(x=>x.QuarterFk.DistrictFk.CityFk.CountryFk).FirstOrDefaultAsync(x=>x.Id==input.Id);

            var output = new GetCompanyForEditOutput { Company = ObjectMapper.Map<CreateOrEditCompanyDto>(company) };

            if (output.Company.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.Company.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }

            output.CityId = company.CityId;
            output.QuarterId= company.QuarterId;
            output.CountryId = company.QuarterFk.DistrictFk.CityFk.CountryId;
            output.DistrictId = company.QuarterFk.DistrictId;

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCompanyDto input)
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

        private string NormalizeCompanyName(string str)
        {
            return str.ToUpper(CultureInfo.GetCultureInfo("en-US"));
        }

        [AbpAuthorize(AppPermissions.Pages_Companies_Create)]
        protected virtual async Task Create(CreateOrEditCompanyDto input)
        {
            var company = ObjectMapper.Map<Company>(input);

            var has = _lookup_organizationUnitRepository.GetAll().AsEnumerable<OrganizationUnit>().Any(x => NormalizeCompanyName(x.DisplayName) == NormalizeCompanyName(input.Name));
            if (has)
            {
                throw new UserFriendlyException("Bu şirket ismi daha önce kullanılmış !");
            }

            var orgUnit = await _organizationUnitAppService.CreateOrganizationUnit(new()
            {
                DisplayName = input.Name
            });

            company.OrganizationUnitId= orgUnit.Id;

            await _companyRepository.InsertAsync(company);

        }

        [AbpAuthorize(AppPermissions.Pages_Companies_Edit)]
        protected virtual async Task Update(CreateOrEditCompanyDto input)
        {
            var company = await _companyRepository.GetAllIncluding(x=>x.OrganizationUnitFk).FirstOrDefaultAsync(x=>x.Id==(int)input.Id);
            if (company.OrganizationUnitFk.DisplayName != input.Name)
            {
                var has = _lookup_organizationUnitRepository.GetAll().AsEnumerable<OrganizationUnit>().Any(x => NormalizeCompanyName(x.DisplayName) == NormalizeCompanyName(input.Name));
                if (has)
                {
                    throw new UserFriendlyException("Bu şirket ismi daha önce kullanılmış !");
                }

                var update = ObjectMapper.Map<UpdateOrganizationUnitInput>(company.OrganizationUnitFk);
                update.DisplayName = input.Name;
                await _organizationUnitAppService.UpdateOrganizationUnit(update);
            }
            ObjectMapper.Map(input, company);

        }

        [AbpAuthorize(AppPermissions.Pages_Companies_Delete)]
        public async Task Delete(EntityDto input)
        {
            var company = await _companyRepository.FirstOrDefaultAsync(x => x.Id == (int)input.Id);

            await _companyRepository.DeleteAsync(input.Id);
            await _lookup_organizationUnitRepository.DeleteAsync(company.OrganizationUnitId);
        }

        public async Task<FileDto> GetCompaniesToExcel(GetAllCompaniesForExcelInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                        ? (CompanyType)input.TypeFilter
                        : default;

            var filteredCompanies = _companyRepository.GetAll()
                        .Include(e => e.OrganizationUnitFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.QuarterFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.TaxAdministration.Contains(input.Filter) || e.TaxNo.Contains(input.Filter) || e.Website.Contains(input.Filter) || e.Phone.Contains(input.Filter) || e.Fax.Contains(input.Filter) || e.Email.Contains(input.Filter) || e.Address.Contains(input.Filter) || e.RunningCode.Contains(input.Filter) || e.Prefix.Contains(input.Filter))
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive))
                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxAdministrationFilter), e => e.TaxAdministration.Contains(input.TaxAdministrationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxNoFilter), e => e.TaxNo.Contains(input.TaxNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WebsiteFilter), e => e.Website.Contains(input.WebsiteFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PhoneFilter), e => e.Phone.Contains(input.PhoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FaxFilter), e => e.Fax.Contains(input.FaxFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmailFilter), e => e.Email.Contains(input.EmailFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressFilter), e => e.Address.Contains(input.AddressFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RunningCodeFilter), e => e.RunningCode.Contains(input.RunningCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrefixFilter), e => e.Prefix.Contains(input.PrefixFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OwnerOrganizationUnitDisplayName), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OwnerOrganizationUnitDisplayName)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityDisplayPropertyFilter), e => string.Format("{0} {1}", e.CityFk == null || e.CityFk.Code == null ? "" : e.CityFk.Code.ToString()
, e.CityFk == null || e.CityFk.Name == null ? "" : e.CityFk.Name.ToString()
) == input.CityDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.QuarterNameFilter), e => e.QuarterFk != null && e.QuarterFk.Name == input.QuarterNameFilter);

            var query = (from o in filteredCompanies
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_cityRepository.GetAll() on o.CityId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_quarterRepository.GetAll() on o.QuarterId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         select new GetCompanyForViewDto()
                         {
                             Company = new CompanyDto
                             {
                                 IsActive = o.IsActive,
                                 Type = o.Type,
                                 TaxAdministration = o.TaxAdministration,
                                 TaxNo = o.TaxNo,
                                 Website = o.Website,
                                 Phone = o.Phone,
                                 Fax = o.Fax,
                                 Email = o.Email,
                                 Address = o.Address,
                                 RunningCode = o.RunningCode,
                                 Prefix = o.Prefix,
                                 Id = o.Id
                             },
                             OrganizationUnitDisplayName = s1 == null || s1.DisplayName == null ? "" : s1.DisplayName.ToString(),
                             CityDisplayProperty = string.Format("{0} {1}", s2 == null || s2.Code == null ? "" : s2.Code.ToString()
, s2 == null || s2.Name == null ? "" : s2.Name.ToString()
),
                             QuarterName = s3 == null || s3.Name == null ? "" : s3.Name.ToString()
                         });

            var companyListDtos = await query.ToListAsync();

            return _companiesExcelExporter.ExportToFile(companyListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Companies)]
        public async Task<PagedResultDto<CompanyOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.DisplayName != null && e.DisplayName.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CompanyOrganizationUnitLookupTableDto>();
            foreach (var organizationUnit in organizationUnitList)
            {
                lookupTableDtoList.Add(new CompanyOrganizationUnitLookupTableDto
                {
                    Id = organizationUnit.Id,
                    DisplayName = organizationUnit.DisplayName?.ToString()
                });
            }

            return new PagedResultDto<CompanyOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_Companies)]
        public async Task<List<CompanyCityLookupTableDto>> GetAllCityForTableDropdown()
        {
            return await _lookup_cityRepository.GetAll()
                .Select(city => new CompanyCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = string.Format("{0} {1}", city.Code, city.Name)
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Companies)]
        public async Task<List<CompanyQuarterLookupTableDto>> GetAllQuarterForTableDropdown()
        {
            return await _lookup_quarterRepository.GetAll()
                .Select(quarter => new CompanyQuarterLookupTableDto
                {
                    Id = quarter.Id,
                    DisplayName = quarter == null || quarter.Name == null ? "" : quarter.Name.ToString()
                }).ToListAsync();
        }

    }
}