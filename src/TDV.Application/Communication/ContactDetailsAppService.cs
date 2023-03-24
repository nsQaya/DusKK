using TDV.Communication;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Communication.Exporting;
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
    [AbpAuthorize(AppPermissions.Pages_ContactDetails)]
    public class ContactDetailsAppService : TDVAppServiceBase, IContactDetailsAppService
    {
        private readonly IRepository<ContactDetail> _contactDetailRepository;
        private readonly IContactDetailsExcelExporter _contactDetailsExcelExporter;
        private readonly IRepository<Contact, int> _lookup_contactRepository;

        public ContactDetailsAppService(IRepository<ContactDetail> contactDetailRepository, IContactDetailsExcelExporter contactDetailsExcelExporter, IRepository<Contact, int> lookup_contactRepository)
        {
            _contactDetailRepository = contactDetailRepository;
            _contactDetailsExcelExporter = contactDetailsExcelExporter;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetContactDetailForViewDto>> GetAll(GetAllContactDetailsInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                        ? (ContactType)input.TypeFilter
                        : default;

            var filteredContactDetails = _contactDetailRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Value.Contains(input.Filter))
                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter), e => e.Value.Contains(input.ValueFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactIdentifyNoFilter), e => e.ContactFk != null && e.ContactFk.IdentifyNo == input.ContactIdentifyNoFilter)
                        .WhereIf(input.ContactIdFilter != null, e => e.ContactId == input.ContactIdFilter);

            var pagedAndFilteredContactDetails = filteredContactDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contactDetails = from o in pagedAndFilteredContactDetails
                                 join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                                 from s1 in j1.DefaultIfEmpty()

                                 select new
                                 {

                                     o.Type,
                                     o.Value,
                                     Id = o.Id,
                                     ContactIdentifyNo = s1 == null || s1.IdentifyNo == null ? "" : s1.IdentifyNo.ToString()
                                 };

            var totalCount = await filteredContactDetails.CountAsync();

            var dbList = await contactDetails.ToListAsync();
            var results = new List<GetContactDetailForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContactDetailForViewDto()
                {
                    ContactDetail = new ContactDetailDto
                    {

                        Type = o.Type,
                        Value = o.Value,
                        Id = o.Id,
                    },
                    ContactIdentifyNo = o.ContactIdentifyNo
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContactDetailForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetContactDetailForViewDto> GetContactDetailForView(int id)
        {
            var contactDetail = await _contactDetailRepository.GetAsync(id);

            var output = new GetContactDetailForViewDto { ContactDetail = ObjectMapper.Map<ContactDetailDto>(contactDetail) };

            if (output.ContactDetail.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.ContactDetail.ContactId);
                output.ContactIdentifyNo = _lookupContact?.IdentifyNo?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ContactDetails_Edit)]
        public async Task<GetContactDetailForEditOutput> GetContactDetailForEdit(EntityDto input)
        {
            var contactDetail = await _contactDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContactDetailForEditOutput { ContactDetail = ObjectMapper.Map<CreateOrEditContactDetailDto>(contactDetail) };

            if (output.ContactDetail.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.ContactDetail.ContactId);
                output.ContactIdentifyNo = _lookupContact?.IdentifyNo?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ContactDetails_Create)]
        protected virtual async Task Create(CreateOrEditContactDetailDto input)
        {
            var contactDetail = ObjectMapper.Map<ContactDetail>(input);

            await _contactDetailRepository.InsertAsync(contactDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactDetails_Edit)]
        protected virtual async Task Update(CreateOrEditContactDetailDto input)
        {
            var contactDetail = await _contactDetailRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, contactDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_ContactDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _contactDetailRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactDetailsToExcel(GetAllContactDetailsForExcelInput input)
        {
            var typeFilter = input.TypeFilter.HasValue
                        ? (ContactType)input.TypeFilter
                        : default;

            var filteredContactDetails = _contactDetailRepository.GetAll()
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Value.Contains(input.Filter))
                        .WhereIf(input.TypeFilter.HasValue && input.TypeFilter > -1, e => e.Type == typeFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ValueFilter), e => e.Value.Contains(input.ValueFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactIdentifyNoFilter), e => e.ContactFk != null && e.ContactFk.IdentifyNo == input.ContactIdentifyNoFilter);

            var query = (from o in filteredContactDetails
                         join o1 in _lookup_contactRepository.GetAll() on o.ContactId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetContactDetailForViewDto()
                         {
                             ContactDetail = new ContactDetailDto
                             {
                                 Type = o.Type,
                                 Value = o.Value,
                                 Id = o.Id
                             },
                             ContactIdentifyNo = s1 == null || s1.IdentifyNo == null ? "" : s1.IdentifyNo.ToString()
                         });

            var contactDetailListDtos = await query.ToListAsync();

            return _contactDetailsExcelExporter.ExportToFile(contactDetailListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ContactDetails)]
        public async Task<PagedResultDto<ContactDetailContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.IdentifyNo != null && e.IdentifyNo.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ContactDetailContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new ContactDetailContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = contact.IdentifyNo?.ToString()
                });
            }

            return new PagedResultDto<ContactDetailContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}