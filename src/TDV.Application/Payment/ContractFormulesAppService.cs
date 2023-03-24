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
    [AbpAuthorize(AppPermissions.Pages_ContractFormules)]
    public class ContractFormulesAppService : TDVAppServiceBase, IContractFormulesAppService
    {
        private readonly IRepository<ContractFormule> _contractFormuleRepository;
        private readonly IContractFormulesExcelExporter _contractFormulesExcelExporter;

        public ContractFormulesAppService(IRepository<ContractFormule> contractFormuleRepository, IContractFormulesExcelExporter contractFormulesExcelExporter)
        {
            _contractFormuleRepository = contractFormuleRepository;
            _contractFormulesExcelExporter = contractFormulesExcelExporter;

        }

        public async Task<PagedResultDto<GetContractFormuleForViewDto>> GetAll(GetAllContractFormulesInput input)
        {

            var filteredContractFormules = _contractFormuleRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Formule.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormuleFilter), e => e.Formule.Contains(input.FormuleFilter));

            var pagedAndFilteredContractFormules = filteredContractFormules
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredContractFormules.CountAsync();

            var dbList = await pagedAndFilteredContractFormules.ToListAsync();

            return new PagedResultDto<GetContractFormuleForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetContractFormuleForViewDto>>(dbList)
            );

        }

        public async Task<GetContractFormuleForViewDto> GetContractFormuleForView(int id)
        {
            var contractFormule = await _contractFormuleRepository.GetAsync(id);

            var output = new GetContractFormuleForViewDto { ContractFormule = ObjectMapper.Map<ContractFormuleDto>(contractFormule) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ContractFormules_Edit)]
        public async Task<GetContractFormuleForEditOutput> GetContractFormuleForEdit(EntityDto input)
        {
            var contractFormule = await _contractFormuleRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContractFormuleForEditOutput { ContractFormule = ObjectMapper.Map<CreateOrEditContractFormuleDto>(contractFormule) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContractFormuleDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ContractFormules_Create)]
        protected virtual async Task Create(CreateOrEditContractFormuleDto input)
        {
            var contractFormule = ObjectMapper.Map<ContractFormule>(input);

            await _contractFormuleRepository.InsertAsync(contractFormule);

        }

        [AbpAuthorize(AppPermissions.Pages_ContractFormules_Edit)]
        protected virtual async Task Update(CreateOrEditContractFormuleDto input)
        {
            var contractFormule = await _contractFormuleRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, contractFormule);

        }

        [AbpAuthorize(AppPermissions.Pages_ContractFormules_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _contractFormuleRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContractFormulesToExcel(GetAllContractFormulesForExcelInput input)
        {

            var filteredContractFormules = _contractFormuleRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Formule.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FormuleFilter), e => e.Formule.Contains(input.FormuleFilter));

            var query = (from o in filteredContractFormules
                         select new GetContractFormuleForViewDto()
                         {
                             ContractFormule = new ContractFormuleDto
                             {
                                 Formule = o.Formule,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var contractFormuleListDtos = await query.ToListAsync();

            return _contractFormulesExcelExporter.ExportToFile(contractFormuleListDtos);
        }

    }
}