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

namespace TDV.Kalite
{
    [AbpAuthorize(AppPermissions.Pages_Stoks)]
    public class StoksAppService : TDVAppServiceBase, IStoksAppService
    {
        private readonly IRepository<Stok> _stokRepository;
        private readonly IStoksExcelExporter _stoksExcelExporter;

        public StoksAppService(IRepository<Stok> stokRepository, IStoksExcelExporter stoksExcelExporter)
        {
            _stokRepository = stokRepository;
            _stoksExcelExporter = stoksExcelExporter;

        }

        public async Task<PagedResultDto<GetStokForViewDto>> GetAll(GetAllStoksInput input)
        {

            var filteredStoks = _stokRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Kodu.Contains(input.Filter) || e.Adi.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.KoduFilter), e => e.Kodu.Contains(input.KoduFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdiFilter), e => e.Adi.Contains(input.AdiFilter));

            var pagedAndFilteredStoks = filteredStoks
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            

            var totalCount = await filteredStoks.CountAsync();

            var dbList = await pagedAndFilteredStoks.ToListAsync();
            var results = new List<GetStokForViewDto>();

            

            return new PagedResultDto<GetStokForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetStokForViewDto>>(dbList)
            );

        }

        public async Task<GetStokForViewDto> GetStokForView(int id)
        {
            var stok = await _stokRepository.GetAsync(id);

            var output = new GetStokForViewDto { Stok = ObjectMapper.Map<StokDto>(stok) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Stoks_Edit)]
        public async Task<GetStokForEditOutput> GetStokForEdit(EntityDto input)
        {
            var stok = await _stokRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStokForEditOutput { Stok = ObjectMapper.Map<CreateOrEditStokDto>(stok) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStokDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Stoks_Create)]
        protected virtual async Task Create(CreateOrEditStokDto input)
        {
            var stok = ObjectMapper.Map<Stok>(input);

            await _stokRepository.InsertAsync(stok);

        }

        [AbpAuthorize(AppPermissions.Pages_Stoks_Edit)]
        protected virtual async Task Update(CreateOrEditStokDto input)
        {
            var stok = await _stokRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, stok);

        }

        [AbpAuthorize(AppPermissions.Pages_Stoks_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _stokRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoksToExcel(GetAllStoksForExcelInput input)
        {

            var filteredStoks = _stokRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Kodu.Contains(input.Filter) || e.Adi.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.KoduFilter), e => e.Kodu.Contains(input.KoduFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AdiFilter), e => e.Adi.Contains(input.AdiFilter));

            var query = (from o in filteredStoks
                         select new GetStokForViewDto()
                         {
                             Stok = new StokDto
                             {
                                 Kodu = o.Kodu,
                                 Adi = o.Adi,
                                 Id = o.Id
                             }
                         });

            var stokListDtos = await query.ToListAsync();

            return _stoksExcelExporter.ExportToFile(stokListDtos);
        }

    }
}