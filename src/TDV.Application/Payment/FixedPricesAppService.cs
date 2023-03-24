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
    [AbpAuthorize(AppPermissions.Pages_FixedPrices)]
    public class FixedPricesAppService : TDVAppServiceBase, IFixedPricesAppService
    {
        private readonly IRepository<FixedPrice> _fixedPriceRepository;
        private readonly IFixedPricesExcelExporter _fixedPricesExcelExporter;

        public FixedPricesAppService(IRepository<FixedPrice> fixedPriceRepository, IFixedPricesExcelExporter fixedPricesExcelExporter)
        {
            _fixedPriceRepository = fixedPriceRepository;
            _fixedPricesExcelExporter = fixedPricesExcelExporter;

        }

        public async Task<PagedResultDto<GetFixedPriceForViewDto>> GetAll(GetAllFixedPricesInput input)
        {

            var filteredFixedPrices = _fixedPriceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter));

            var pagedAndFilteredFixedPrices = filteredFixedPrices
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredFixedPrices.CountAsync();

            var dbList = await pagedAndFilteredFixedPrices.ToListAsync();

            return new PagedResultDto<GetFixedPriceForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFixedPriceForViewDto>>(dbList)
            );

        }

        public async Task<GetFixedPriceForViewDto> GetFixedPriceForView(int id)
        {
            var fixedPrice = await _fixedPriceRepository.GetAsync(id);

            var output = new GetFixedPriceForViewDto { FixedPrice = ObjectMapper.Map<FixedPriceDto>(fixedPrice) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FixedPrices_Edit)]
        public async Task<GetFixedPriceForEditOutput> GetFixedPriceForEdit(EntityDto input)
        {
            var fixedPrice = await _fixedPriceRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFixedPriceForEditOutput { FixedPrice = ObjectMapper.Map<CreateOrEditFixedPriceDto>(fixedPrice) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFixedPriceDto input)
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

        [AbpAuthorize(AppPermissions.Pages_FixedPrices_Create)]
        protected virtual async Task Create(CreateOrEditFixedPriceDto input)
        {
            var fixedPrice = ObjectMapper.Map<FixedPrice>(input);

            await _fixedPriceRepository.InsertAsync(fixedPrice);

        }

        [AbpAuthorize(AppPermissions.Pages_FixedPrices_Edit)]
        protected virtual async Task Update(CreateOrEditFixedPriceDto input)
        {
            var fixedPrice = await _fixedPriceRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, fixedPrice);

        }

        [AbpAuthorize(AppPermissions.Pages_FixedPrices_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _fixedPriceRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFixedPricesToExcel(GetAllFixedPricesForExcelInput input)
        {

            var filteredFixedPrices = _fixedPriceRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter));

            var query = (from o in filteredFixedPrices
                         select new GetFixedPriceForViewDto()
                         {
                             FixedPrice = new FixedPriceDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var fixedPriceListDtos = await query.ToListAsync();

            return _fixedPricesExcelExporter.ExportToFile(fixedPriceListDtos);
        }

    }
}