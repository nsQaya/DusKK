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
    [AbpAuthorize(AppPermissions.Pages_FixedPriceDetails)]
    public class FixedPriceDetailsAppService : TDVAppServiceBase, IFixedPriceDetailsAppService
    {
        private readonly IRepository<FixedPriceDetail> _fixedPriceDetailRepository;
        private readonly IFixedPriceDetailsExcelExporter _fixedPriceDetailsExcelExporter;
        private readonly IRepository<FixedPrice, int> _lookup_fixedPriceRepository;

        public FixedPriceDetailsAppService(IRepository<FixedPriceDetail> fixedPriceDetailRepository, IFixedPriceDetailsExcelExporter fixedPriceDetailsExcelExporter, IRepository<FixedPrice, int> lookup_fixedPriceRepository)
        {
            _fixedPriceDetailRepository = fixedPriceDetailRepository;
            _fixedPriceDetailsExcelExporter = fixedPriceDetailsExcelExporter;
            _lookup_fixedPriceRepository = lookup_fixedPriceRepository;

        }

        public async Task<PagedResultDto<GetFixedPriceDetailForViewDto>> GetAll(GetAllFixedPriceDetailsInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                        ? (PaymentMethodType)input.TypeFilter
                        : PaymentMethodType.KM;

            var currencyTypeFilter = input.CurrencyTypeFilter.HasValue
                ? (CurrencyType)input.CurrencyTypeFilter
                : default;

            var filteredFixedPriceDetails = _fixedPriceDetailRepository.GetAll()
                        .Include(e => e.FixedPriceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.CurrencyTypeFilter.HasValue && input.CurrencyTypeFilter > -1, e => e.CurrencyType == currencyTypeFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FixedPriceNameFilter), e => e.FixedPriceFk != null && e.FixedPriceFk.Name == input.FixedPriceNameFilter);

            var pagedAndFilteredFixedPriceDetails = filteredFixedPriceDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);



            var totalCount = await filteredFixedPriceDetails.CountAsync();

            var dbList = await pagedAndFilteredFixedPriceDetails.ToListAsync();
            
            return new PagedResultDto<GetFixedPriceDetailForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFixedPriceDetailForViewDto>>(dbList)
            );

        }

        public async Task<GetFixedPriceDetailForViewDto> GetFixedPriceDetailForView(int id)
        {
            var fixedPriceDetail = await _fixedPriceDetailRepository.GetAsync(id);

            var output = new GetFixedPriceDetailForViewDto { FixedPriceDetail = ObjectMapper.Map<FixedPriceDetailDto>(fixedPriceDetail) };

            if (output.FixedPriceDetail.FixedPriceId != null)
            {
                var _lookupFixedPrice = await _lookup_fixedPriceRepository.FirstOrDefaultAsync((int)output.FixedPriceDetail.FixedPriceId);
                output.FixedPriceName = _lookupFixedPrice?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FixedPriceDetails_Edit)]
        public async Task<GetFixedPriceDetailForEditOutput> GetFixedPriceDetailForEdit(EntityDto input)
        {
            var fixedPriceDetail = await _fixedPriceDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFixedPriceDetailForEditOutput { FixedPriceDetail = ObjectMapper.Map<CreateOrEditFixedPriceDetailDto>(fixedPriceDetail) };

            if (output.FixedPriceDetail.FixedPriceId != null)
            {
                var _lookupFixedPrice = await _lookup_fixedPriceRepository.FirstOrDefaultAsync((int)output.FixedPriceDetail.FixedPriceId);
                output.FixedPriceName = _lookupFixedPrice?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFixedPriceDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_FixedPriceDetails_Create)]
        protected virtual async Task Create(CreateOrEditFixedPriceDetailDto input)
        {
            var fixedPriceDetail = ObjectMapper.Map<FixedPriceDetail>(input);

            await _fixedPriceDetailRepository.InsertAsync(fixedPriceDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_FixedPriceDetails_Edit)]
        protected virtual async Task Update(CreateOrEditFixedPriceDetailDto input)
        {
            var fixedPriceDetail = await _fixedPriceDetailRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, fixedPriceDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_FixedPriceDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _fixedPriceDetailRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFixedPriceDetailsToExcel(GetAllFixedPriceDetailsForExcelInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                        ? (PaymentMethodType)input.TypeFilter
                        : default;
            var currencyTypeFilter = input.CurrencyTypeFilter.HasValue
                ? (CurrencyType)input.CurrencyTypeFilter
                : default;

            var filteredFixedPriceDetails = _fixedPriceDetailRepository.GetAll()
                        .Include(e => e.FixedPriceFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.CurrencyTypeFilter.HasValue && input.CurrencyTypeFilter > -1, e => e.CurrencyType == currencyTypeFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FixedPriceNameFilter), e => e.FixedPriceFk != null && e.FixedPriceFk.Name == input.FixedPriceNameFilter);

            var query = (from o in filteredFixedPriceDetails
                         join o1 in _lookup_fixedPriceRepository.GetAll() on o.FixedPriceId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetFixedPriceDetailForViewDto()
                         {
                             FixedPriceDetail = new FixedPriceDetailDto
                             {
                                 Type = o.Type,
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 CurrencyType = o.CurrencyType,
                                 Price = o.Price,
                                 Id = o.Id
                             },
                             FixedPriceName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var fixedPriceDetailListDtos = await query.ToListAsync();

            return _fixedPriceDetailsExcelExporter.ExportToFile(fixedPriceDetailListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_FixedPriceDetails)]
        public async Task<PagedResultDto<FixedPriceDetailFixedPriceLookupTableDto>> GetAllFixedPriceForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_fixedPriceRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var fixedPriceList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<FixedPriceDetailFixedPriceLookupTableDto>();
            foreach (var fixedPrice in fixedPriceList)
            {
                lookupTableDtoList.Add(new FixedPriceDetailFixedPriceLookupTableDto
                {
                    Id = fixedPrice.Id,
                    DisplayName = fixedPrice.Name?.ToString()
                });
            }

            return new PagedResultDto<FixedPriceDetailFixedPriceLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}