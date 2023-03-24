using TDV.Corporation;
using TDV.Communication;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Corporation.Exporting;
using TDV.Corporation.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;

namespace TDV.Corporation
{
    [AbpAuthorize(AppPermissions.Pages_CompanyContacts)]
    public class CompanyContactsAppService : TDVAppServiceBase, ICompanyContactsAppService
    {
        private readonly IRepository<CompanyContact> _companyContactRepository;
        private readonly ICompanyContactsExcelExporter _companyContactsExcelExporter;
        private readonly IRepository<Company, int> _lookup_companyRepository;
        private readonly IRepository<Contact, int> _lookup_contactRepository;

        public CompanyContactsAppService(IRepository<CompanyContact> companyContactRepository, ICompanyContactsExcelExporter companyContactsExcelExporter, IRepository<Company, int> lookup_companyRepository, IRepository<Contact, int> lookup_contactRepository)
        {
            _companyContactRepository = companyContactRepository;
            _companyContactsExcelExporter = companyContactsExcelExporter;
            _lookup_companyRepository = lookup_companyRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetCompanyContactForViewDto>> GetAll(GetAllCompanyContactsInput input)
        {

            var filteredCompanyContacts = _companyContactRepository.GetAll()
                        .Include(e => e.CompanyFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyDisplayPropertyFilter), e => string.Format("{0} {1}", e.CompanyFk == null || e.CompanyFk.TaxAdministration == null ? "" : e.CompanyFk.TaxAdministration.ToString()
, e.CompanyFk == null || e.CompanyFk.RunningCode == null ? "" : e.CompanyFk.RunningCode.ToString()
) == input.CompanyDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNameFilter), e => e.ContactFk != null && e.ContactFk.Name == input.ContactNameFilter)
                        .WhereIf(input.OrganizationUnitId > 0, x => x.CompanyFk.OrganizationUnitId == input.OrganizationUnitId);

            var pagedAndFilteredCompanyContacts = filteredCompanyContacts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredCompanyContacts.CountAsync();

            var dbList = await pagedAndFilteredCompanyContacts.ToListAsync();
           
            return new PagedResultDto<GetCompanyContactForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetCompanyContactForViewDto>>(dbList)
            );

        }

        public async Task<GetCompanyContactForViewDto> GetCompanyContactForView(int id)
        {
            var companyContact = await _companyContactRepository.GetAsync(id);

            var output = new GetCompanyContactForViewDto { CompanyContact = ObjectMapper.Map<CompanyContactDto>(companyContact) };

            if (output.CompanyContact.CompanyId != null)
            {
                var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.CompanyContact.CompanyId);
                output.CompanyDisplayProperty = string.Format("{0} {1}", _lookupCompany.TaxAdministration, _lookupCompany.RunningCode);
            }

            if (output.CompanyContact.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.CompanyContact.ContactId);
                output.ContactName = _lookupContact?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CompanyContacts_Edit)]
        public async Task<GetCompanyContactForEditOutput> GetCompanyContactForEdit(EntityDto input)
        {
            var companyContact = await _companyContactRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCompanyContactForEditOutput { CompanyContact = ObjectMapper.Map<CreateOrEditCompanyContactDto>(companyContact) };

            if (output.CompanyContact.CompanyId != null)
            {
                var _lookupCompany = await _lookup_companyRepository.FirstOrDefaultAsync((int)output.CompanyContact.CompanyId);
                output.CompanyDisplayProperty = string.Format("{0} {1}", _lookupCompany.TaxAdministration, _lookupCompany.RunningCode);
            }

            if (output.CompanyContact.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.CompanyContact.ContactId);
                output.ContactName = _lookupContact?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCompanyContactDto input)
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

        [AbpAuthorize(AppPermissions.Pages_CompanyContacts_Create)]
        protected virtual async Task Create(CreateOrEditCompanyContactDto input)
        {
            var companyContact = ObjectMapper.Map<CompanyContact>(input);

            await _companyContactRepository.InsertAsync(companyContact);

        }

        [AbpAuthorize(AppPermissions.Pages_CompanyContacts_Edit)]
        protected virtual async Task Update(CreateOrEditCompanyContactDto input)
        {
            var companyContact = await _companyContactRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, companyContact);

        }

        [AbpAuthorize(AppPermissions.Pages_CompanyContacts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _companyContactRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCompanyContactsToExcel(GetAllCompanyContactsForExcelInput input)
        {

            var filteredCompanyContacts = _companyContactRepository.GetAll()
                        .Include(e => e.CompanyFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CompanyDisplayPropertyFilter), e => string.Format("{0} {1}", e.CompanyFk == null || e.CompanyFk.TaxAdministration == null ? "" : e.CompanyFk.TaxAdministration.ToString()
, e.CompanyFk == null || e.CompanyFk.RunningCode == null ? "" : e.CompanyFk.RunningCode.ToString()
) == input.CompanyDisplayPropertyFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNameFilter), e => e.ContactFk != null && e.ContactFk.Name == input.ContactNameFilter);

            var query = (from o in filteredCompanyContacts
                         join o1 in _lookup_companyRepository.GetAll() on o.CompanyId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetCompanyContactForViewDto()
                         {
                             CompanyContact = new CompanyContactDto
                             {
                                 Title = o.Title,
                                 Id = o.Id
                             },
                             CompanyDisplayProperty = string.Format("{0} {1}", s1 == null || s1.TaxAdministration == null ? "" : s1.TaxAdministration.ToString()
, s1 == null || s1.RunningCode == null ? "" : s1.RunningCode.ToString()
),
                             ContactName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var companyContactListDtos = await query.ToListAsync();

            return _companyContactsExcelExporter.ExportToFile(companyContactListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_CompanyContacts)]
        public async Task<PagedResultDto<CompanyContactCompanyLookupTableDto>> GetAllCompanyForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_companyRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => string.Format("{0} {1}", e.TaxAdministration, e.RunningCode).Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var companyList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CompanyContactCompanyLookupTableDto>();
            foreach (var company in companyList)
            {
                lookupTableDtoList.Add(new CompanyContactCompanyLookupTableDto
                {
                    Id = company.Id,
                    DisplayName = string.Format("{0} {1}", company.TaxAdministration, company.RunningCode)
                });
            }

            return new PagedResultDto<CompanyContactCompanyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_CompanyContacts)]
        public async Task<PagedResultDto<CompanyContactContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<CompanyContactContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new CompanyContactContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.Name?.ToString()
                });
            }

            return new PagedResultDto<CompanyContactContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}