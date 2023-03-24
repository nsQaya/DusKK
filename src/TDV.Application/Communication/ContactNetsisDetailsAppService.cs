using TDV.Communication;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Communication.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;

namespace TDV.Communication
{
    [AbpAuthorize(AppPermissions.Pages_ContactNetsisDetails)]
    public class ContactNetsisDetailsAppService : TDVAppServiceBase, IContactNetsisDetailsAppService
    {
        private readonly IRepository<ContactNetsisDetail> _contactNetsisDetailRepository;
        private readonly IRepository<Contact, int> _lookup_contactRepository;

        public ContactNetsisDetailsAppService(IRepository<ContactNetsisDetail> contactNetsisDetailRepository, IRepository<Contact, int> lookup_contactRepository)
        {
            _contactNetsisDetailRepository = contactNetsisDetailRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetContactNetsisDetailForViewDto>> GetAll(GetAllContactNetsisDetailsInput input)
        {

            var filteredContactNetsisDetails = _contactNetsisDetailRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.NetsisNo.Contains(input.Filter) || e.RegistryNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NetsisNoFilter), e => e.NetsisNo.Contains(input.NetsisNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegistryNoFilter), e => e.RegistryNo.Contains(input.RegistryNoFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactNameFilter), e => e.ContactFk != null && e.ContactFk.Name == input.ContactNameFilter);

            var pagedAndFilteredContactNetsisDetails = filteredContactNetsisDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contactNetsisDetails = from o in pagedAndFilteredContactNetsisDetails
                                       join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       select new
                                       {

                                           o.NetsisNo,
                                           o.RegistryNo,
                                           Id = o.Id,
                                           ContactName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                       };

            var totalCount = await filteredContactNetsisDetails.CountAsync();

            var dbList = await contactNetsisDetails.ToListAsync();
            var results = new List<GetContactNetsisDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContactNetsisDetailForViewDto()
                {
                    ContactNetsisDetail = new ContactNetsisDetailDto
                    {

                        NetsisNo = o.NetsisNo,
                        RegistryNo = o.RegistryNo,
                        Id = o.Id,
                    },
                    ContactName = o.ContactName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContactNetsisDetailForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_ContactNetsisDetails_Edit)]
        public async Task<GetContactNetsisDetailForEditOutput> GetContactNetsisDetailForEdit(EntityDto input)
        {
            var contactNetsisDetail = await _contactNetsisDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactNetsisDetailForEditOutput { ContactNetsisDetail = ObjectMapper.Map<CreateOrEditContactNetsisDetailDto>(contactNetsisDetail) };

            if (output.ContactNetsisDetail.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.ContactNetsisDetail.ContactId);
                output.ContactName = _lookupContact?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactNetsisDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ContactNetsisDetails_Create)]
        protected virtual async Task Create(CreateOrEditContactNetsisDetailDto input)
        {
            var contactNetsisDetail = ObjectMapper.Map<ContactNetsisDetail>(input);

            await _contactNetsisDetailRepository.InsertAsync(contactNetsisDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactNetsisDetails_Edit)]
        protected virtual async Task Update(CreateOrEditContactNetsisDetailDto input)
        {
            var contactNetsisDetail = await _contactNetsisDetailRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, contactNetsisDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactNetsisDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _contactNetsisDetailRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_ContactNetsisDetails)]
        public async Task<PagedResultDto<ContactNetsisDetailContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactNetsisDetailContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ContactNetsisDetailContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.Name?.ToString()
                });
            }

            return new PagedResultDto<ContactNetsisDetailContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}