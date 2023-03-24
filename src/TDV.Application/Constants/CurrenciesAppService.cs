using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Constants.Exporting;
using TDV.Constants.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;
using TDV.Constants.Dtos;
using TDV.Constants;
using TDV.Burial.Dtos;

namespace TDV.Constans
{
    [AbpAuthorize(AppPermissions.Pages_Currencies)]
    public class CurrenciesAppService : TDVAppServiceBase, ICurrenciesAppService
    {
        private readonly IRepository<Currency> _currencyRepository;
        private readonly ICurrenciesExcelExporter _currenciesExcelExporter;

        public CurrenciesAppService(IRepository<Currency> currencyRepository, ICurrenciesExcelExporter currenciesExcelExporter)
        {
            _currencyRepository = currencyRepository;
            _currenciesExcelExporter = currenciesExcelExporter;

        }

        public async Task<PagedResultDto<GetCurrencyForViewDto>> GetAll(GetAllCurrenciesInput input)
        {

            var filteredCurrencies = _currencyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Symbol.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SymbolFilter), e => e.Symbol.Contains(input.SymbolFilter))
                        .WhereIf(input.MinOrderNumberFilter != null, e => e.OrderNumber >= input.MinOrderNumberFilter)
                        .WhereIf(input.MaxOrderNumberFilter != null, e => e.OrderNumber <= input.MaxOrderNumberFilter)
                        .WhereIf(input.MinNumberCodeFilter != null, e => e.NumberCode >= input.MinNumberCodeFilter)
                        .WhereIf(input.MaxNumberCodeFilter != null, e => e.NumberCode <= input.MaxNumberCodeFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var pagedAndFilteredCurrencies = filteredCurrencies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredCurrencies.CountAsync();

            var dbList = await pagedAndFilteredCurrencies.ToListAsync();
            var results = new List<GetCurrencyForViewDto>();


            return new PagedResultDto<GetCurrencyForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetCurrencyForViewDto>>(dbList)
            );

        }

        public async Task<GetCurrencyForViewDto> GetCurrencyForView(int id)
        {
            var currency = await _currencyRepository.GetAsync(id);

            var output = new GetCurrencyForViewDto { Currency = ObjectMapper.Map<CurrencyDto>(currency) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Currencies_Edit)]
        public async Task<GetCurrencyForEditOutput> GetCurrencyForEdit(EntityDto input)
        {
            var currency = await _currencyRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCurrencyForEditOutput { Currency = ObjectMapper.Map<CreateOrEditCurrencyDto>(currency) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCurrencyDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Currencies_Create)]
        protected virtual async Task Create(CreateOrEditCurrencyDto input)
        {
            var currency = ObjectMapper.Map<Currency>(input);

            await _currencyRepository.InsertAsync(currency);

        }

        [AbpAuthorize(AppPermissions.Pages_Currencies_Edit)]
        protected virtual async Task Update(CreateOrEditCurrencyDto input)
        {
            var currency = await _currencyRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, currency);

        }

        [AbpAuthorize(AppPermissions.Pages_Currencies_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _currencyRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCurrenciesToExcel(GetAllCurrenciesForExcelInput input)
        {

            var filteredCurrencies = _currencyRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Code.Contains(input.Filter) || e.Symbol.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CodeFilter), e => e.Code.Contains(input.CodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SymbolFilter), e => e.Symbol.Contains(input.SymbolFilter))
                        .WhereIf(input.MinOrderNumberFilter != null, e => e.OrderNumber >= input.MinOrderNumberFilter)
                        .WhereIf(input.MaxOrderNumberFilter != null, e => e.OrderNumber <= input.MaxOrderNumberFilter)
                        .WhereIf(input.MinNumberCodeFilter != null, e => e.NumberCode >= input.MinNumberCodeFilter)
                        .WhereIf(input.MaxNumberCodeFilter != null, e => e.NumberCode <= input.MaxNumberCodeFilter)
                        .WhereIf(input.IsActiveFilter.HasValue && input.IsActiveFilter > -1, e => (input.IsActiveFilter == 1 && e.IsActive) || (input.IsActiveFilter == 0 && !e.IsActive));

            var query = (from o in filteredCurrencies
                         select new GetCurrencyForViewDto()
                         {
                             Currency = new CurrencyDto
                             {
                                 Code = o.Code,
                                 Symbol = o.Symbol,
                                 OrderNumber = o.OrderNumber,
                                 NumberCode = o.NumberCode,
                                 IsActive = o.IsActive,
                                 Id = o.Id
                             }
                         });

            var currencyListDtos = await query.ToListAsync();

            return _currenciesExcelExporter.ExportToFile(currencyListDtos);
        }

    }
}