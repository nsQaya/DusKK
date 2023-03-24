using TDV.Location;
using TDV.Corporation;

using TDV.Payment;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Payment.Exporting;
using TDV.Payment.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;

namespace TDV.Payment
{
    [AbpAuthorize(AppPermissions.Pages_Contracts)]
    public class ContractsAppService : TDVAppServiceBase, IContractsAppService
    {
        private readonly IRepository<Contract> _contractRepository;
        private readonly IContractsExcelExporter _contractsExcelExporter;
        private readonly IRepository<Region, int> _lookup_regionRepository;
        private readonly IRepository<Company, int> _lookup_companyRepository;

        public ContractsAppService(IRepository<Contract> contractRepository, IContractsExcelExporter contractsExcelExporter, IRepository<Region, int> lookup_regionRepository, IRepository<Company, int> lookup_companyRepository)
        {
            _contractRepository = contractRepository;
            _contractsExcelExporter = contractsExcelExporter;
            _lookup_regionRepository = lookup_regionRepository;
            _lookup_companyRepository = lookup_companyRepository;

        }

        public async Task<PagedResultDto<GetContractForViewDto>> GetAll(GetAllContractsInput input)
        {
            var currencyTypeFilter = input.CurrencyTypeFilter.HasValue
                        ? (CurrencyType)input.CurrencyTypeFilter
                        : default;

            var filteredContracts = _contractRepository.GetAll()
                        .Include(e => e.RegionFk)
                        .Include(e => e.CompanyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Formule.Contains(input.Filter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.CurrencyTypeFilter.HasValue && input.CurrencyTypeFilter > -1, e => e.CurrencyType == currencyTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionNameFilter), e => e.RegionFk != null && e.RegionFk.Name == input.RegionNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyDisplayPropertyFilter), e => string.Format("{0} {1}", e.CompanyFk == null || e.CompanyFk.TaxNo == null ? "" : e.CompanyFk.TaxNo.ToString()
, e.CompanyFk == null || e.CompanyFk.RunningCode == null ? "" : e.CompanyFk.RunningCode.ToString()
) == input.CompanyDisplayPropertyFilter);

            var pagedAndFilteredContracts = filteredContracts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredContracts.CountAsync();

            var dbList = await pagedAndFilteredContracts.ToListAsync();
            

            return new PagedResultDto<GetContractForViewDto>(
                totalCount,
                 ObjectMapper.Map<List<GetContractForViewDto>>(dbList)
            );

        }

        public async Task<GetContractForViewDto> GetContractForView(int id)
        {
            var contract = await _contractRepository.GetAsync(id);

            var output = new GetContractForViewDto { Contract = ObjectMapper.Map<ContractDto>(contract) };

            if (output.Contract.RegionId != null)
            {
                var _lookupRegion = await _lookup_regionRepository.FirstOrDefaultAsync((int)output.Contract.RegionId);
                output.RegionName = _lookupRegion?.Name?.ToString();
            }

            if (output.Contract.CompanyId != null)
            {
                var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.Contract.CompanyId);
                output.CompanyDisplayProperty = string.Format("{0} {1}", _lookupCompany.TaxNo, _lookupCompany.RunningCode);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Contracts_Edit)]
        public async Task<GetContractForEditOutput> GetContractForEdit(EntityDto input)
        {
            var contract = await _contractRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContractForEditOutput { Contract = ObjectMapper.Map<CreateOrEditContractDto>(contract) };

            if (output.Contract.RegionId != null)
            {
                var _lookupRegion = await _lookup_regionRepository.FirstOrDefaultAsync((int)output.Contract.RegionId);
                output.RegionName = _lookupRegion?.Name?.ToString();
            }

            if (output.Contract.CompanyId != null)
            {
                var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.Contract.CompanyId);
                output.CompanyDisplayProperty = string.Format("{0} {1}", _lookupCompany.TaxNo, _lookupCompany.RunningCode);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContractDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Contracts_Create)]
        protected virtual async Task Create(CreateOrEditContractDto input)
        {
            var contract = ObjectMapper.Map<Contract>(input);

            await _contractRepository.InsertAsync(contract);

        }

        [AbpAuthorize(AppPermissions.Pages_Contracts_Edit)]
        protected virtual async Task Update(CreateOrEditContractDto input)
        {
            var contract = await _contractRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, contract);

        }

        [AbpAuthorize(AppPermissions.Pages_Contracts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _contractRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContractsToExcel(GetAllContractsForExcelInput input)
        {
            var currencyTypeFilter = input.CurrencyTypeFilter.HasValue
                        ? (CurrencyType)input.CurrencyTypeFilter
                        : default;

            var filteredContracts = _contractRepository.GetAll()
                        .Include(e => e.RegionFk)
                        .Include(e => e.CompanyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Formule.Contains(input.Filter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.CurrencyTypeFilter.HasValue && input.CurrencyTypeFilter > -1, e => e.CurrencyType == currencyTypeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionNameFilter), e => e.RegionFk != null && e.RegionFk.Name == input.RegionNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyDisplayPropertyFilter), e => string.Format("{0} {1}", e.CompanyFk == null || e.CompanyFk.TaxNo == null ? "" : e.CompanyFk.TaxNo.ToString()
, e.CompanyFk == null || e.CompanyFk.RunningCode == null ? "" : e.CompanyFk.RunningCode.ToString()
) == input.CompanyDisplayPropertyFilter);

            var query = (from o in filteredContracts
                         join o1 in _lookup_regionRepository.GetAll() on o.RegionId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_companyRepository.GetAll() on o.CompanyId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetContractForViewDto()
                         {
                             Contract = new ContractDto
                             {
                                 Formule = o.Formule,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 CurrencyType = o.CurrencyType,
                                 Id = o.Id
                             },
                             RegionName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             CompanyDisplayProperty = string.Format("{0} {1}", s2 == null || s2.TaxNo == null ? "" : s2.TaxNo.ToString()
, s2 == null || s2.RunningCode == null ? "" : s2.RunningCode.ToString()
)
                         });

            var contractListDtos = await query.ToListAsync();

            return _contractsExcelExporter.ExportToFile(contractListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Contracts)]
        public async Task<List<ContractRegionLookupTableDto>> GetAllRegionForTableDropdown()
        {
            return await _lookup_regionRepository.GetAll()
                .Select(region => new ContractRegionLookupTableDto
                {
                    Id = region.Id,
                    DisplayName = region == null || region.Name == null ? "" : region.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_Contracts)]
        public async Task<List<ContractCompanyLookupTableDto>> GetAllCompanyForTableDropdown()
        {
            return await _lookup_companyRepository.GetAll()
                .Select(company => new ContractCompanyLookupTableDto
                {
                    Id = company.Id,
                    DisplayName = string.Format("{0} {1}", company.TaxNo, company.RunningCode)
                }).ToListAsync();
        }

    }
}