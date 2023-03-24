using TDV.Burial;

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
    [AbpAuthorize(AppPermissions.Pages_FuneralTranportOrders)]
    public class FuneralTranportOrdersAppService : TDVAppServiceBase, IFuneralTranportOrdersAppService
    {
        private readonly IRepository<FuneralTranportOrder> _funeralTranportOrderRepository;
        private readonly IFuneralTranportOrdersExcelExporter _funeralTranportOrdersExcelExporter;

        public FuneralTranportOrdersAppService(IRepository<FuneralTranportOrder> funeralTranportOrderRepository, IFuneralTranportOrdersExcelExporter funeralTranportOrdersExcelExporter)
        {
            _funeralTranportOrderRepository = funeralTranportOrderRepository;
            _funeralTranportOrdersExcelExporter = funeralTranportOrdersExcelExporter;

        }

        public async Task<PagedResultDto<GetFuneralTranportOrderForViewDto>> GetAll(GetAllFuneralTranportOrdersInput input)
        {

            var filteredFuneralTranportOrders = _funeralTranportOrderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReceiverFullName.Contains(input.Filter) || e.ReceiverKinshipDegree.Contains(input.Filter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinStartKMFilter != null, e => e.StartKM >= input.MinStartKMFilter)
                        .WhereIf(input.MaxStartKMFilter != null, e => e.StartKM <= input.MaxStartKMFilter)
                        .WhereIf(input.MinOperationDateFilter != null, e => e.OperationDate >= input.MinOperationDateFilter)
                        .WhereIf(input.MaxOperationDateFilter != null, e => e.OperationDate <= input.MaxOperationDateFilter)
                        .WhereIf(input.MinOperationKMFilter != null, e => e.OperationKM >= input.MinOperationKMFilter)
                        .WhereIf(input.MaxOperationKMFilter != null, e => e.OperationKM <= input.MaxOperationKMFilter)
                        .WhereIf(input.MinDeliveryDateFilter != null, e => e.DeliveryDate >= input.MinDeliveryDateFilter)
                        .WhereIf(input.MaxDeliveryDateFilter != null, e => e.DeliveryDate <= input.MaxDeliveryDateFilter)
                        .WhereIf(input.MinDeliveryKMFilter != null, e => e.DeliveryKM >= input.MinDeliveryKMFilter)
                        .WhereIf(input.MaxDeliveryKMFilter != null, e => e.DeliveryKM <= input.MaxDeliveryKMFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinEndKMFilter != null, e => e.EndKM >= input.MinEndKMFilter)
                        .WhereIf(input.MaxEndKMFilter != null, e => e.EndKM <= input.MaxEndKMFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReceiverFullNameFilter), e => e.ReceiverFullName.Contains(input.ReceiverFullNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReceiverKinshipDegreeFilter), e => e.ReceiverKinshipDegree.Contains(input.ReceiverKinshipDegreeFilter));

            var pagedAndFilteredFuneralTranportOrders = filteredFuneralTranportOrders
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredFuneralTranportOrders.CountAsync();

            var dbList = await pagedAndFilteredFuneralTranportOrders.ToListAsync();
            

            return new PagedResultDto<GetFuneralTranportOrderForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetFuneralTranportOrderForViewDto>>(dbList)
            );

        }

        public async Task<GetFuneralTranportOrderForViewDto> GetFuneralTranportOrderForView(int id)
        {
            var funeralTranportOrder = await _funeralTranportOrderRepository.GetAsync(id);

            var output = new GetFuneralTranportOrderForViewDto { FuneralTranportOrder = ObjectMapper.Map<FuneralTranportOrderDto>(funeralTranportOrder) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_FuneralTranportOrders_Edit)]
        public async Task<GetFuneralTranportOrderForEditOutput> GetFuneralTranportOrderForEdit(EntityDto input)
        {
            var funeralTranportOrder = await _funeralTranportOrderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetFuneralTranportOrderForEditOutput { FuneralTranportOrder = ObjectMapper.Map<CreateOrEditFuneralTranportOrderDto>(funeralTranportOrder) };


            return output;
        }

        public async Task CreateOrEdit(CreateOrEditFuneralTranportOrderDto input)
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

        [AbpAuthorize(AppPermissions.Pages_FuneralTranportOrders_Create)]
        protected virtual async Task Create(CreateOrEditFuneralTranportOrderDto input)
        {
            var funeralTranportOrder = ObjectMapper.Map<FuneralTranportOrder>(input);

            await _funeralTranportOrderRepository.InsertAsync(funeralTranportOrder);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralTranportOrders_Edit)]
        protected virtual async Task Update(CreateOrEditFuneralTranportOrderDto input)
        {
            var funeralTranportOrder = await _funeralTranportOrderRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, funeralTranportOrder);

        }

        [AbpAuthorize(AppPermissions.Pages_FuneralTranportOrders_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _funeralTranportOrderRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetFuneralTranportOrdersToExcel(GetAllFuneralTranportOrdersForExcelInput input)
        {

            var filteredFuneralTranportOrders = _funeralTranportOrderRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.ReceiverFullName.Contains(input.Filter) || e.ReceiverKinshipDegree.Contains(input.Filter))
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinStartKMFilter != null, e => e.StartKM >= input.MinStartKMFilter)
                        .WhereIf(input.MaxStartKMFilter != null, e => e.StartKM <= input.MaxStartKMFilter)
                        .WhereIf(input.MinOperationDateFilter != null, e => e.OperationDate >= input.MinOperationDateFilter)
                        .WhereIf(input.MaxOperationDateFilter != null, e => e.OperationDate <= input.MaxOperationDateFilter)
                        .WhereIf(input.MinOperationKMFilter != null, e => e.OperationKM >= input.MinOperationKMFilter)
                        .WhereIf(input.MaxOperationKMFilter != null, e => e.OperationKM <= input.MaxOperationKMFilter)
                        .WhereIf(input.MinDeliveryDateFilter != null, e => e.DeliveryDate >= input.MinDeliveryDateFilter)
                        .WhereIf(input.MaxDeliveryDateFilter != null, e => e.DeliveryDate <= input.MaxDeliveryDateFilter)
                        .WhereIf(input.MinDeliveryKMFilter != null, e => e.DeliveryKM >= input.MinDeliveryKMFilter)
                        .WhereIf(input.MaxDeliveryKMFilter != null, e => e.DeliveryKM <= input.MaxDeliveryKMFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinEndKMFilter != null, e => e.EndKM >= input.MinEndKMFilter)
                        .WhereIf(input.MaxEndKMFilter != null, e => e.EndKM <= input.MaxEndKMFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReceiverFullNameFilter), e => e.ReceiverFullName.Contains(input.ReceiverFullNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ReceiverKinshipDegreeFilter), e => e.ReceiverKinshipDegree.Contains(input.ReceiverKinshipDegreeFilter));

            var query = (from o in filteredFuneralTranportOrders

                         select new GetFuneralTranportOrderForViewDto()
                         {
                             FuneralTranportOrder = new FuneralTranportOrderDto
                             {
                                 StartDate = o.StartDate,
                                 StartKM = o.StartKM,
                                 OperationDate = o.OperationDate,
                                 OperationKM = o.OperationKM,
                                 DeliveryDate = o.DeliveryDate,
                                 DeliveryKM = o.DeliveryKM,
                                 EndDate = o.EndDate,
                                 EndKM = o.EndKM,
                                 ReceiverFullName = o.ReceiverFullName,
                                 ReceiverKinshipDegree = o.ReceiverKinshipDegree,
                                 Id = o.Id
                             },
                             
                         });

            var funeralTranportOrderListDtos = await query.ToListAsync();

            return _funeralTranportOrdersExcelExporter.ExportToFile(funeralTranportOrderListDtos);
        }

    }
}