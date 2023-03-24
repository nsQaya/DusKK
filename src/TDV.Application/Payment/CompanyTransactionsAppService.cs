using TDV.Corporation;
using TDV.Burial;
using TDV.Constants;

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
using TDV.Burial.Dtos;

namespace TDV.Payment
{
    [AbpAuthorize(AppPermissions.Pages_CompanyTransactions)]
    public class CompanyTransactionsAppService : TDVAppServiceBase, ICompanyTransactionsAppService
    {
        private readonly IRepository<CompanyTransaction> _companyTransactionRepository;
        private readonly ICompanyTransactionsExcelExporter _companyTransactionsExcelExporter;
        private readonly IRepository<Company, int> _lookup_companyRepository;
        private readonly IRepository<Funeral, int> _lookup_funeralRepository;
        private readonly IRepository<DataList, int> _lookup_dataListRepository;
        private readonly IRepository<Currency, int> _lookup_currencyRepository;

        public CompanyTransactionsAppService(IRepository<CompanyTransaction> companyTransactionRepository, ICompanyTransactionsExcelExporter companyTransactionsExcelExporter, IRepository<Company, int> lookup_companyRepository, IRepository<Funeral, int> lookup_funeralRepository, IRepository<DataList, int> lookup_dataListRepository, IRepository<Currency, int> lookup_currencyRepository)
        {
            _companyTransactionRepository = companyTransactionRepository;
            _companyTransactionsExcelExporter = companyTransactionsExcelExporter;
            _lookup_companyRepository = lookup_companyRepository;
            _lookup_funeralRepository = lookup_funeralRepository;
            _lookup_dataListRepository = lookup_dataListRepository;
            _lookup_currencyRepository = lookup_currencyRepository;

        }

        public async Task<PagedResultDto<GetCompanyTransactionForViewDto>> GetAll(GetAllCompanyTransactionsInput input)
        {

            var filteredCompanyTransactions = _companyTransactionRepository.GetAll()
                        .Include(e => e.CompanyFk)
                        .Include(e => e.FuneralFk)
                        .Include(e => e.TypeFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.UnitTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.InOut.Contains(input.Filter) || e.No.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InOutFilter), e => e.InOut.Contains(input.InOutFilter))
                        .WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
                        .WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoFilter), e => e.No.Contains(input.NoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        .WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        .WhereIf(input.MinTotalFilter != null, e => e.Total >= input.MinTotalFilter)
                        .WhereIf(input.MaxTotalFilter != null, e => e.Total <= input.MaxTotalFilter)
                        .WhereIf(input.IsTransferredFilter.HasValue && input.IsTransferredFilter > -1, e => (input.IsTransferredFilter == 1 && e.IsTransferred) || (input.IsTransferredFilter == 0 && !e.IsTransferred))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyTaxAdministrationFilter), e => e.CompanyFk != null && e.CompanyFk.TaxAdministration == input.CompanyTaxAdministrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.FuneralFk == null || e.FuneralFk.TransferNo == null ? "" : e.FuneralFk.TransferNo.ToString()
, e.FuneralFk == null || e.FuneralFk.Name == null ? "" : e.FuneralFk.Name.ToString()
, e.FuneralFk == null || e.FuneralFk.Surname == null ? "" : e.FuneralFk.Surname.ToString()
) == input.FuneralDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DataListValueFilter), e => e.TypeFk != null && e.TypeFk.Value == input.DataListValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeFilter), e => e.CurrencyFk != null && e.CurrencyFk.Code == input.CurrencyCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DataListValue2Filter), e => e.UnitTypeFk != null && e.UnitTypeFk.Value == input.DataListValue2Filter);

            var pagedAndFilteredCompanyTransactions = filteredCompanyTransactions
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredCompanyTransactions.CountAsync();

            var dbList = await filteredCompanyTransactions.ToListAsync();            

            return new PagedResultDto<GetCompanyTransactionForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetCompanyTransactionForViewDto>>(dbList)
            );

        }

        public async Task<GetCompanyTransactionForViewDto> GetCompanyTransactionForView(int id)
        {
            var companyTransaction = await _companyTransactionRepository.GetAsync(id);

            var output = new GetCompanyTransactionForViewDto { CompanyTransaction = ObjectMapper.Map<CompanyTransactionDto>(companyTransaction) };

            if (output.CompanyTransaction.CompanyId != null)
            {
                var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.CompanyId);
                output.CompanyTaxAdministration = _lookupCompany?.TaxAdministration?.ToString();
            }

            if (output.CompanyTransaction.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.FuneralId);
                output.FuneralDisplayProperty = string.Format("{0} {1} {2}", _lookupFuneral.TransferNo, _lookupFuneral.Name, _lookupFuneral.Surname);
            }

            if (output.CompanyTransaction.Type != null)
            {
                var _lookupDataList = await _lookup_dataListRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.Type);
                output.DataListValue = _lookupDataList?.Value?.ToString();
            }

            if (output.CompanyTransaction.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.CurrencyId);
                output.CurrencyCode = _lookupCurrency?.Code?.ToString();
            }

            if (output.CompanyTransaction.UnitType != null)
            {
                var _lookupDataList = await _lookup_dataListRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.UnitType);
                output.DataListValue2 = _lookupDataList?.Value?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CompanyTransactions_Edit)]
        public async Task<GetCompanyTransactionForEditOutput> GetCompanyTransactionForEdit(EntityDto input)
        {
            var companyTransaction = await _companyTransactionRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCompanyTransactionForEditOutput { CompanyTransaction = ObjectMapper.Map<CreateOrEditCompanyTransactionDto>(companyTransaction) };

            if (output.CompanyTransaction.CompanyId != null)
            {
                var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.CompanyId);
                output.CompanyTaxAdministration = _lookupCompany?.TaxAdministration?.ToString();
            }

            if (output.CompanyTransaction.FuneralId != null)
            {
                var _lookupFuneral = await _lookup_funeralRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.FuneralId);
                output.FuneralDisplayProperty = string.Format("{0} {1} {2}", _lookupFuneral.TransferNo, _lookupFuneral.Name, _lookupFuneral.Surname);
            }

            if (output.CompanyTransaction.Type != null)
            {
                var _lookupDataList = await _lookup_dataListRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.Type);
                output.DataListValue = _lookupDataList?.Value?.ToString();
            }

            if (output.CompanyTransaction.CurrencyId != null)
            {
                var _lookupCurrency = await _lookup_currencyRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.CurrencyId);
                output.CurrencyCode = _lookupCurrency?.Code?.ToString();
            }

            if (output.CompanyTransaction.UnitType != null)
            {
                var _lookupDataList = await _lookup_dataListRepository.FirstOrDefaultAsync((int)output.CompanyTransaction.UnitType);
                output.DataListValue2 = _lookupDataList?.Value?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCompanyTransactionDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CompanyTransactions_Create)]
        protected virtual async Task Create(CreateOrEditCompanyTransactionDto input)
        {
            var companyTransaction = ObjectMapper.Map<CompanyTransaction>(input);

            await _companyTransactionRepository.InsertAsync(companyTransaction);

        }

        [AbpAuthorize(AppPermissions.Pages_CompanyTransactions_Edit)]
        protected virtual async Task Update(CreateOrEditCompanyTransactionDto input)
        {
            var companyTransaction = await _companyTransactionRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, companyTransaction);

        }

        [AbpAuthorize(AppPermissions.Pages_CompanyTransactions_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _companyTransactionRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCompanyTransactionsToExcel(GetAllCompanyTransactionsForExcelInput input)
        {

            var filteredCompanyTransactions = _companyTransactionRepository.GetAll()
                        .Include(e => e.CompanyFk)
                        .Include(e => e.FuneralFk)
                        .Include(e => e.TypeFk)
                        .Include(e => e.CurrencyFk)
                        .Include(e => e.UnitTypeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.InOut.Contains(input.Filter) || e.No.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.InOutFilter), e => e.InOut.Contains(input.InOutFilter))
                        .WhereIf(input.MinDateFilter != null, e => e.Date >= input.MinDateFilter)
                        .WhereIf(input.MaxDateFilter != null, e => e.Date <= input.MaxDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoFilter), e => e.No.Contains(input.NoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinTaxRateFilter != null, e => e.TaxRate >= input.MinTaxRateFilter)
                        .WhereIf(input.MaxTaxRateFilter != null, e => e.TaxRate <= input.MaxTaxRateFilter)
                        .WhereIf(input.MinTotalFilter != null, e => e.Total >= input.MinTotalFilter)
                        .WhereIf(input.MaxTotalFilter != null, e => e.Total <= input.MaxTotalFilter)
                        .WhereIf(input.IsTransferredFilter.HasValue && input.IsTransferredFilter > -1, e => (input.IsTransferredFilter == 1 && e.IsTransferred) || (input.IsTransferredFilter == 0 && !e.IsTransferred))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyTaxAdministrationFilter), e => e.CompanyFk != null && e.CompanyFk.TaxAdministration == input.CompanyTaxAdministrationFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FuneralDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.FuneralFk == null || e.FuneralFk.TransferNo == null ? "" : e.FuneralFk.TransferNo.ToString()
, e.FuneralFk == null || e.FuneralFk.Name == null ? "" : e.FuneralFk.Name.ToString()
, e.FuneralFk == null || e.FuneralFk.Surname == null ? "" : e.FuneralFk.Surname.ToString()
) == input.FuneralDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DataListValueFilter), e => e.TypeFk != null && e.TypeFk.Value == input.DataListValueFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CurrencyCodeFilter), e => e.CurrencyFk != null && e.CurrencyFk.Code == input.CurrencyCodeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DataListValue2Filter), e => e.UnitTypeFk != null && e.UnitTypeFk.Value == input.DataListValue2Filter);

            var query = (from o in filteredCompanyTransactions
                         join o1 in _lookup_companyRepository.GetAll() on o.CompanyId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_funeralRepository.GetAll() on o.FuneralId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_dataListRepository.GetAll() on o.Type equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_currencyRepository.GetAll() on o.CurrencyId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_dataListRepository.GetAll() on o.UnitType equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetCompanyTransactionForViewDto()
                         {
                             CompanyTransaction = new CompanyTransactionDto
                             {
                                 InOut = o.InOut,
                                 Date = o.Date,
                                 No = o.No,
                                 Description = o.Description,
                                 Amount = o.Amount,
                                 Price = o.Price,
                                 TaxRate = o.TaxRate,
                                 Total = o.Total,
                                 IsTransferred = o.IsTransferred,
                                 Id = o.Id
                             },
                             CompanyTaxAdministration = s1 == null || s1.TaxAdministration == null ? "" : s1.TaxAdministration.ToString(),
                             FuneralDisplayProperty = string.Format("{0} {1} {2}", s2 == null || s2.TransferNo == null ? "" : s2.TransferNo.ToString()
, s2 == null || s2.Name == null ? "" : s2.Name.ToString()
, s2 == null || s2.Surname == null ? "" : s2.Surname.ToString()
),
                             DataListValue = s3 == null || s3.Value == null ? "" : s3.Value.ToString(),
                             CurrencyCode = s4 == null || s4.Code == null ? "" : s4.Code.ToString(),
                             DataListValue2 = s5 == null || s5.Value == null ? "" : s5.Value.ToString()
                         });

            var companyTransactionListDtos = await query.ToListAsync();

            return _companyTransactionsExcelExporter.ExportToFile(companyTransactionListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CompanyTransactions)]
        public async Task<List<CompanyTransactionCompanyLookupTableDto>> GetAllCompanyForTableDropdown()
        {
            return await _lookup_companyRepository.GetAll()
                .Select(company => new CompanyTransactionCompanyLookupTableDto
                {
                    Id = company.Id,
                    DisplayName = company == null || company.TaxAdministration == null ? "" : company.TaxAdministration.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CompanyTransactions)]
        public async Task<List<CompanyTransactionFuneralLookupTableDto>> GetAllFuneralForTableDropdown()
        {
            return await _lookup_funeralRepository.GetAll()
                .Select(funeral => new CompanyTransactionFuneralLookupTableDto
                {
                    Id = funeral.Id,
                    DisplayName = string.Format("{0} {1} {2}", funeral.TransferNo, funeral.Name, funeral.Surname)
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CompanyTransactions)]
        public async Task<List<CompanyTransactionDataListLookupTableDto>> GetAllDataListForTableDropdown()
        {
            return await _lookup_dataListRepository.GetAll()
                .Select(dataList => new CompanyTransactionDataListLookupTableDto
                {
                    Id = dataList.Id,
                    DisplayName = dataList == null || dataList.Value == null ? "" : dataList.Value.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_CompanyTransactions)]
        public async Task<List<CompanyTransactionCurrencyLookupTableDto>> GetAllCurrencyForTableDropdown()
        {
            return await _lookup_currencyRepository.GetAll()
                .Select(currency => new CompanyTransactionCurrencyLookupTableDto
                {
                    Id = currency.Id,
                    DisplayName = currency == null || currency.Code == null ? "" : currency.Code.ToString()
                }).ToListAsync();
        }

    }
}