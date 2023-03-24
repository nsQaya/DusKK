using TDV.Kalite;

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
    [AbpAuthorize(AppPermissions.Pages_StokOlcus)]
    public class StokOlcusAppService : TDVAppServiceBase, IStokOlcusAppService
    {
        private readonly IRepository<StokOlcu> _stokOlcuRepository;
        private readonly IStokOlcusExcelExporter _stokOlcusExcelExporter;
        private readonly IRepository<Stok, int> _lookup_stokRepository;
        private readonly IRepository<Olcum, int> _lookup_olcumRepository;

        public StokOlcusAppService(IRepository<StokOlcu> stokOlcuRepository, IStokOlcusExcelExporter stokOlcusExcelExporter, IRepository<Stok, int> lookup_stokRepository, IRepository<Olcum, int> lookup_olcumRepository)
        {
            _stokOlcuRepository = stokOlcuRepository;
            _stokOlcusExcelExporter = stokOlcusExcelExporter;
            _lookup_stokRepository = lookup_stokRepository;
            _lookup_olcumRepository = lookup_olcumRepository;

        }

        public async Task<PagedResultDto<GetStokOlcuForViewDto>> GetAll(GetAllStokOlcusInput input)
        {

            var filteredStokOlcus = _stokOlcuRepository.GetAll()
                        .Include(e => e.StokFk)
                        .Include(e => e.OlcumFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Deger.Contains(input.Filter))
                        .WhereIf(input.MinAltFilter != null, e => e.Alt >= input.MinAltFilter)
                        .WhereIf(input.MaxAltFilter != null, e => e.Alt <= input.MaxAltFilter)
                        .WhereIf(input.MinUstFilter != null, e => e.Ust >= input.MinUstFilter)
                        .WhereIf(input.MaxUstFilter != null, e => e.Ust <= input.MaxUstFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DegerFilter), e => e.Deger.Contains(input.DegerFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StokAdiFilter), e => e.StokFk != null && e.StokFk.Adi == input.StokAdiFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OlcumOlcuTipiFilter), e => e.OlcumFk != null && e.OlcumFk.OlcuTipi == input.OlcumOlcuTipiFilter);

            var pagedAndFilteredStokOlcus = filteredStokOlcus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            

            var totalCount = await filteredStokOlcus.CountAsync();

            var dbList = await pagedAndFilteredStokOlcus.ToListAsync();
            var results = new List<GetStokOlcuForViewDto>();

            

            return new PagedResultDto<GetStokOlcuForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetStokOlcuForViewDto>>(dbList)
            );

        }

        public async Task<GetStokOlcuForViewDto> GetStokOlcuForView(int id)
        {
            var stokOlcu = await _stokOlcuRepository.GetAsync(id);

            var output = new GetStokOlcuForViewDto { StokOlcu = ObjectMapper.Map<StokOlcuDto>(stokOlcu) };

            if (output.StokOlcu.StokId != null)
            {
                var _lookupStok = await _lookup_stokRepository.FirstOrDefaultAsync((int)output.StokOlcu.StokId);
                output.StokAdi = _lookupStok?.Adi?.ToString();
            }

            if (output.StokOlcu.OlcumId != null)
            {
                var _lookupOlcum = await _lookup_olcumRepository.FirstOrDefaultAsync((int)output.StokOlcu.OlcumId);
                output.OlcumOlcuTipi = _lookupOlcum?.OlcuTipi?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StokOlcus_Edit)]
        public async Task<GetStokOlcuForEditOutput> GetStokOlcuForEdit(EntityDto input)
        {
            var stokOlcu = await _stokOlcuRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStokOlcuForEditOutput { StokOlcu = ObjectMapper.Map<CreateOrEditStokOlcuDto>(stokOlcu) };

            if (output.StokOlcu.StokId != null)
            {
                var _lookupStok = await _lookup_stokRepository.FirstOrDefaultAsync((int)output.StokOlcu.StokId);
                output.StokAdi = _lookupStok?.Adi?.ToString();
            }

            if (output.StokOlcu.OlcumId != null)
            {
                var _lookupOlcum = await _lookup_olcumRepository.FirstOrDefaultAsync((int)output.StokOlcu.OlcumId);
                output.OlcumOlcuTipi = _lookupOlcum?.OlcuTipi?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStokOlcuDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StokOlcus_Create)]
        protected virtual async Task Create(CreateOrEditStokOlcuDto input)
        {
            var stokOlcu = ObjectMapper.Map<StokOlcu>(input);

            await _stokOlcuRepository.InsertAsync(stokOlcu);

        }

        [AbpAuthorize(AppPermissions.Pages_StokOlcus_Edit)]
        protected virtual async Task Update(CreateOrEditStokOlcuDto input)
        {
            var stokOlcu = await _stokOlcuRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, stokOlcu);

        }

        [AbpAuthorize(AppPermissions.Pages_StokOlcus_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _stokOlcuRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStokOlcusToExcel(GetAllStokOlcusForExcelInput input)
        {

            var filteredStokOlcus = _stokOlcuRepository.GetAll()
                        .Include(e => e.StokFk)
                        .Include(e => e.OlcumFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Deger.Contains(input.Filter))
                        .WhereIf(input.MinAltFilter != null, e => e.Alt >= input.MinAltFilter)
                        .WhereIf(input.MaxAltFilter != null, e => e.Alt <= input.MaxAltFilter)
                        .WhereIf(input.MinUstFilter != null, e => e.Ust >= input.MinUstFilter)
                        .WhereIf(input.MaxUstFilter != null, e => e.Ust <= input.MaxUstFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DegerFilter), e => e.Deger.Contains(input.DegerFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StokAdiFilter), e => e.StokFk != null && e.StokFk.Adi == input.StokAdiFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OlcumOlcuTipiFilter), e => e.OlcumFk != null && e.OlcumFk.OlcuTipi == input.OlcumOlcuTipiFilter);

            var query = (from o in filteredStokOlcus
                         join o1 in _lookup_stokRepository.GetAll() on o.StokId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_olcumRepository.GetAll() on o.OlcumId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStokOlcuForViewDto()
                         {
                             StokOlcu = new StokOlcuDto
                             {
                                 Alt = o.Alt,
                                 Ust = o.Ust,
                                 Deger = o.Deger,
                                 Id = o.Id
                             },
                             StokAdi = s1 == null || s1.Adi == null ? "" : s1.Adi.ToString(),
                             OlcumOlcuTipi = s2 == null || s2.OlcuTipi == null ? "" : s2.OlcuTipi.ToString()
                         });

            var stokOlcuListDtos = await query.ToListAsync();

            return _stokOlcusExcelExporter.ExportToFile(stokOlcuListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StokOlcus)]
        public async Task<PagedResultDto<StokOlcuStokLookupTableDto>> GetAllStokForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_stokRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Adi != null && e.Adi.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var stokList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StokOlcuStokLookupTableDto>();
            foreach (var stok in stokList)
            {
                lookupTableDtoList.Add(new StokOlcuStokLookupTableDto
                {
                    Id = stok.Id,
                    DisplayName = stok.Adi?.ToString()
                });
            }

            return new PagedResultDto<StokOlcuStokLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_StokOlcus)]
        public async Task<List<StokOlcuOlcumLookupTableDto>> GetAllOlcumForTableDropdown()
        {
            return await _lookup_olcumRepository.GetAll()
                .Select(olcum => new StokOlcuOlcumLookupTableDto
                {
                    Id = olcum.Id,
                    DisplayName = olcum == null || olcum.OlcuTipi == null ? "" : olcum.OlcuTipi.ToString()
                }).ToListAsync();
        }

    }
}