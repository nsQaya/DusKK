using TDV.Kalite;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Rapor.Exporting;
using TDV.Rapor.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;
using System.Security.AccessControl;

namespace TDV.Rapor
{
    [AbpAuthorize(AppPermissions.Pages_Taleps)]
    public class TalepsAppService : TDVAppServiceBase, ITalepsAppService
    {
        private readonly IRepository<Talep> _talepRepository;
        private readonly ITalepsExcelExporter _talepsExcelExporter;
        private readonly IRepository<Stok, int> _lookup_stokRepository;

        public TalepsAppService(IRepository<Talep> talepRepository, ITalepsExcelExporter talepsExcelExporter, IRepository<Stok, int> lookup_stokRepository)
        {
            _talepRepository = talepRepository;
            _talepsExcelExporter = talepsExcelExporter;
            _lookup_stokRepository = lookup_stokRepository;

        }

        public async Task<PagedResultDto<GetTalepForViewDto>> GetAll(GetAllTalepsInput input)
        {

            var filteredTaleps = _talepRepository.GetAll()
                        .Include(e => e.StokFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OlcuBr.Contains(input.Filter))
                        .WhereIf(input.MinTalepMiktarFilter != null, e => e.TalepMiktar >= input.MinTalepMiktarFilter)
                        .WhereIf(input.MaxTalepMiktarFilter != null, e => e.TalepMiktar <= input.MaxTalepMiktarFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OlcuBrFilter), e => e.OlcuBr.Contains(input.OlcuBrFilter))
                        .WhereIf(input.MinFiyatFilter != null, e => e.Fiyat >= input.MinFiyatFilter)
                        .WhereIf(input.MaxFiyatFilter != null, e => e.Fiyat <= input.MaxFiyatFilter)
                        .WhereIf(input.MinTutarFilter != null, e => e.Tutar >= input.MinTutarFilter)
                        .WhereIf(input.MaxTutarFilter != null, e => e.Tutar <= input.MaxTutarFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StokAdiFilter), e => e.StokFk != null && e.StokFk.Adi == input.StokAdiFilter);

            var pagedAndFilteredTaleps = filteredTaleps
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

           

            var totalCount = await filteredTaleps.CountAsync();

            var dbList = await pagedAndFilteredTaleps.ToListAsync();
            var results = new List<GetTalepForViewDto>();

           

            return new PagedResultDto<GetTalepForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetTalepForViewDto>>(dbList)
            );

        }

        public async Task<GetTalepForViewDto> GetTalepForView(int id)
        {
            var talep = await _talepRepository.GetAsync(id);

            var output = new GetTalepForViewDto { Talep = ObjectMapper.Map<TalepDto>(talep) };

            if (output.Talep.StokId != null)
            {
                var _lookupStok = await _lookup_stokRepository.FirstOrDefaultAsync((int)output.Talep.StokId);
                output.StokAdi = _lookupStok?.Adi?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Taleps_Edit)]
        public async Task<GetTalepForEditOutput> GetTalepForEdit(EntityDto input)
        {
            var talep = await _talepRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTalepForEditOutput { Talep = ObjectMapper.Map<CreateOrEditTalepDto>(talep) };

            if (output.Talep.StokId != null)
            {
                var _lookupStok = await _lookup_stokRepository.FirstOrDefaultAsync((int)output.Talep.StokId);
                output.StokAdi = _lookupStok?.Adi?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTalepDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Taleps_Create)]
        protected virtual async Task Create(CreateOrEditTalepDto input)
        {
            var talep = ObjectMapper.Map<Talep>(input);

            await _talepRepository.InsertAsync(talep);

        }

        [AbpAuthorize(AppPermissions.Pages_Taleps_Edit)]
        protected virtual async Task Update(CreateOrEditTalepDto input)
        {
            var talep = await _talepRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, talep);

        }

        [AbpAuthorize(AppPermissions.Pages_Taleps_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _talepRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTalepsToExcel(GetAllTalepsForExcelInput input)
        {

            var filteredTaleps = _talepRepository.GetAll()
                        .Include(e => e.StokFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OlcuBr.Contains(input.Filter))
                        .WhereIf(input.MinTalepMiktarFilter != null, e => e.TalepMiktar >= input.MinTalepMiktarFilter)
                        .WhereIf(input.MaxTalepMiktarFilter != null, e => e.TalepMiktar <= input.MaxTalepMiktarFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OlcuBrFilter), e => e.OlcuBr.Contains(input.OlcuBrFilter))
                        .WhereIf(input.MinFiyatFilter != null, e => e.Fiyat >= input.MinFiyatFilter)
                        .WhereIf(input.MaxFiyatFilter != null, e => e.Fiyat <= input.MaxFiyatFilter)
                        .WhereIf(input.MinTutarFilter != null, e => e.Tutar >= input.MinTutarFilter)
                        .WhereIf(input.MaxTutarFilter != null, e => e.Tutar <= input.MaxTutarFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StokAdiFilter), e => e.StokFk != null && e.StokFk.Adi == input.StokAdiFilter);

            var query = (from o in filteredTaleps
                         join o1 in _lookup_stokRepository.GetAll() on o.StokId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetTalepForViewDto()
                         {
                             Talep = new TalepDto
                             {
                                 TalepMiktar = o.TalepMiktar,
                                 OlcuBr = o.OlcuBr,
                                 Fiyat = o.Fiyat,
                                 Tutar = o.Tutar,
                                 Id = o.Id
                             },
                             StokAdi = s1 == null || s1.Adi == null ? "" : s1.Adi.ToString()
                         });

            var talepListDtos = await query.ToListAsync();

            return _talepsExcelExporter.ExportToFile(talepListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_Taleps)]
        public async Task<List<TalepStokLookupTableDto>> GetAllStokForTableDropdown()
        {
            return await _lookup_stokRepository.GetAll()
                .Select(stok => new TalepStokLookupTableDto
                {
                    Id = stok.Id,
                    DisplayName = stok == null || stok.Adi == null ? "" : stok.Adi.ToString()
                }).ToListAsync();
        }

    }
}