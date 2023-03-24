using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Kalite.Exporting;
using TDV.Kalite.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;
using TDV.Payment.Dtos;

namespace TDV.Kalite
{
    [AbpAuthorize(AppPermissions.Pages_Olcums)]
    public class OlcumsAppService : TDVAppServiceBase, IOlcumsAppService
    {
        private readonly IRepository<Olcum> _olcumRepository;
        private readonly IOlcumsExcelExporter _olcumsExcelExporter;

        public OlcumsAppService(IRepository<Olcum> olcumRepository, IOlcumsExcelExporter olcumsExcelExporter)
        {
            _olcumRepository = olcumRepository;
            _olcumsExcelExporter = olcumsExcelExporter;

        }

        public async Task<PagedResultDto<GetOlcumForViewDto>> GetAll(GetAllOlcumsInput input)
        {

            var filteredOlcums = _olcumRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OlcuTipi.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OlcuTipiFilter), e => e.OlcuTipi.Contains(input.OlcuTipiFilter));

            var pagedAndFilteredOlcums = filteredOlcums
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredOlcums.CountAsync();

            var dbList = await pagedAndFilteredOlcums.ToListAsync();
    

            

            return new PagedResultDto<GetOlcumForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetOlcumForViewDto>>(dbList)
            );

        }

        public async Task<GetOlcumForViewDto> GetOlcumForView(int id)
        {
            var olcum = await _olcumRepository.GetAsync(id);

            var output = new GetOlcumForViewDto { Olcum = ObjectMapper.Map<OlcumDto>(olcum) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Olcums_Edit)]
        public async Task<GetOlcumForEditOutput> GetOlcumForEdit(EntityDto input)
        {
            var olcum = await _olcumRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOlcumForEditOutput { Olcum = ObjectMapper.Map<CreateOrEditOlcumDto>(olcum) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOlcumDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Olcums_Create)]
        protected virtual async Task Create(CreateOrEditOlcumDto input)
        {
            var olcum = ObjectMapper.Map<Olcum>(input);

            await _olcumRepository.InsertAsync(olcum);

        }

        [AbpAuthorize(AppPermissions.Pages_Olcums_Edit)]
        protected virtual async Task Update(CreateOrEditOlcumDto input)
        {
            var olcum = await _olcumRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, olcum);

        }

        [AbpAuthorize(AppPermissions.Pages_Olcums_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _olcumRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOlcumsToExcel(GetAllOlcumsForExcelInput input)
        {

            var filteredOlcums = _olcumRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OlcuTipi.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OlcuTipiFilter), e => e.OlcuTipi.Contains(input.OlcuTipiFilter));

            var query = (from o in filteredOlcums
                         select new GetOlcumForViewDto()
                         {
                             Olcum = new OlcumDto
                             {
                                 OlcuTipi = o.OlcuTipi,
                                 Id = o.Id
                             }
                         });

            var olcumListDtos = await query.ToListAsync();

            return _olcumsExcelExporter.ExportToFile(olcumListDtos);
        }

    }
}