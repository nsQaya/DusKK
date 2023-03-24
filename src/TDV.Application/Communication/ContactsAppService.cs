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
    [AbpAuthorize(AppPermissions.Pages_Contacts)]
    public class ContactsAppService : TDVAppServiceBase, IContactsAppService
    {
        private readonly IRepository<Contact> _contactRepository;
        private readonly IContactsExcelExporter _contactsExcelExporter;

        public ContactsAppService(IRepository<Contact> contactRepository, IContactsExcelExporter contactsExcelExporter)
        {
            _contactRepository = contactRepository;
            _contactsExcelExporter = contactsExcelExporter;

        }

        public async Task<PagedResultDto<GetContactForViewDto>> GetAll(GetAllContactsInput input)
        {

            var filteredContacts = _contactRepository.GetAllIncluding(x => x.NetsisDetail)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Surname.Contains(input.Filter) || e.IdentifyNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurnameFilter), e => e.Surname.Contains(input.SurnameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IdentifyNoFilter), e => e.IdentifyNo.Contains(input.IdentifyNoFilter));
                        

            var pagedAndFilteredContacts = filteredContacts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);


            var totalCount = await filteredContacts.CountAsync();

            var dbList = await pagedAndFilteredContacts.ToListAsync();
            
            return new PagedResultDto<GetContactForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetContactForViewDto>>(dbList)
            );

        }

        public async Task<GetContactForViewDto> GetContactForView(int id)
        {
            var contact = await _contactRepository.GetAsync(id);

            var output = new GetContactForViewDto { Contact = ObjectMapper.Map<ContactDto>(contact) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Edit)]
        public async Task<GetContactForEditOutput> GetContactForEdit(EntityDto input)
        {
            var contact = await _contactRepository.GetAllIncluding(x=>x.NetsisDetail).FirstOrDefaultAsync(x=>x.Id==input.Id);

            var output = new GetContactForEditOutput { Contact = ObjectMapper.Map<CreateOrEditContactDto>(contact) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContactDto input)
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

        [AbpAuthorize(AppPermissions.Pages_Contacts_Create)]
        protected virtual async Task Create(CreateOrEditContactDto input)
        {
            var contact = ObjectMapper.Map<Contact>(input);

            await _contactRepository.InsertAsync(contact);

        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Create)]
        public virtual async Task<int> CreateAndGet(CreateOrEditContactDto input)
        {
            var contact = ObjectMapper.Map<Contact>(input);

            return await _contactRepository.InsertAndGetIdAsync(contact);

        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Edit)]
        protected virtual async Task Update(CreateOrEditContactDto input)
        {
            var contact = await _contactRepository.GetAllIncluding(x=>x.NetsisDetail).FirstOrDefaultAsync(x=>x.Id==input.Id);
            ObjectMapper.Map(input, contact);
        }

        [AbpAuthorize(AppPermissions.Pages_Contacts_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _contactRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContactsToExcel(GetAllContactsForExcelInput input)
        {

            var filteredContacts = _contactRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Surname.Contains(input.Filter) || e.IdentifyNo.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SurnameFilter), e => e.Surname.Contains(input.SurnameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IdentifyNoFilter), e => e.IdentifyNo.Contains(input.IdentifyNoFilter));

            var query = (from o in filteredContacts
                         select new GetContactForViewDto()
                         {
                             Contact = new ContactDto
                             {
                                 Name = o.Name,
                                 Surname = o.Surname,
                                 IdentifyNo = o.IdentifyNo,
                                 Id = o.Id
                             }
                         });

            var contactListDtos = await query.ToListAsync();

            return _contactsExcelExporter.ExportToFile(contactListDtos);
        }

    }
}