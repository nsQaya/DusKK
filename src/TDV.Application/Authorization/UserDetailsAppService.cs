using TDV.Authorization.Users;
using TDV.Communication;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TDV.Authorization.Exporting;
using TDV.Authorization.Dtos;
using TDV.Dto;
using Abp.Application.Services.Dto;
using TDV.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TDV.Storage;

namespace TDV.Authorization
{
    [AbpAuthorize(AppPermissions.Pages_UserDetails)]
    public class UserDetailsAppService : TDVAppServiceBase, IUserDetailsAppService
    {
        private readonly IRepository<UserDetail> _userDetailRepository;
        private readonly IUserDetailsExcelExporter _userDetailsExcelExporter;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Contact, int> _lookup_contactRepository;

        public UserDetailsAppService(IRepository<UserDetail> userDetailRepository, IUserDetailsExcelExporter userDetailsExcelExporter, IRepository<User, long> lookup_userRepository, IRepository<Contact, int> lookup_contactRepository)
        {
            _userDetailRepository = userDetailRepository;
            _userDetailsExcelExporter = userDetailsExcelExporter;
            _lookup_userRepository = lookup_userRepository;
            _lookup_contactRepository = lookup_contactRepository;

        }

        public async Task<PagedResultDto<GetUserDetailForViewDto>> GetAll(GetAllUserDetailsInput input)
        {

            var filteredUserDetails = _userDetailRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.ContactFk == null || e.ContactFk.Name == null ? "" : e.ContactFk.Name.ToString()
, e.ContactFk == null || e.ContactFk.Surname == null ? "" : e.ContactFk.Surname.ToString()
, e.ContactFk == null || e.ContactFk.IdentifyNo == null ? "" : e.ContactFk.IdentifyNo.ToString()
) == input.ContactDisplayPropertyFilter);

            var pagedAndFilteredUserDetails = filteredUserDetails
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredUserDetails.CountAsync();

            var dbList = await pagedAndFilteredUserDetails.ToListAsync();

            return new PagedResultDto<GetUserDetailForViewDto>(
                totalCount,
                ObjectMapper.Map<List<GetUserDetailForViewDto>>(dbList)
            );

        }

        public async Task<GetUserDetailForViewDto> GetUserDetailForView(int id)
        {
            var userDetail = await _userDetailRepository.GetAsync(id);

            var output = new GetUserDetailForViewDto { UserDetail = ObjectMapper.Map<UserDetailDto>(userDetail) };

            if (output.UserDetail.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDetail.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.UserDetail.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.UserDetail.ContactId);
                output.ContactDisplayProperty = string.Format("{0} {1} {2}", _lookupContact.Name, _lookupContact.Surname, _lookupContact.IdentifyNo);
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_UserDetails_Edit)]
        public async Task<GetUserDetailForEditOutput> GetUserDetailForEdit(EntityDto input)
        {
            var userDetail = await _userDetailRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetUserDetailForEditOutput { UserDetail = ObjectMapper.Map<CreateOrEditUserDetailDto>(userDetail) };

            if (output.UserDetail.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDetail.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.UserDetail.ContactId != null)
            {
                var _lookupContact = await _lookup_contactRepository.FirstOrDefaultAsync((int)output.UserDetail.ContactId);
                output.ContactDisplayProperty = string.Format("{0} {1} {2}", _lookupContact.Name, _lookupContact.Surname, _lookupContact.IdentifyNo);
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditUserDetailDto input)
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

        [AbpAuthorize(AppPermissions.Pages_UserDetails_Create)]
        protected virtual async Task Create(CreateOrEditUserDetailDto input)
        {
            var userDetail = ObjectMapper.Map<UserDetail>(input);

            await _userDetailRepository.InsertAsync(userDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_UserDetails_Edit)]
        protected virtual async Task Update(CreateOrEditUserDetailDto input)
        {
            var userDetail = await _userDetailRepository.FirstOrDefaultAsync((int)input.Id);
            ObjectMapper.Map(input, userDetail);

        }

        [AbpAuthorize(AppPermissions.Pages_UserDetails_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _userDetailRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetUserDetailsToExcel(GetAllUserDetailsForExcelInput input)
        {

            var filteredUserDetails = _userDetailRepository.GetAll()
                        .Include(e => e.UserFk)
                        .Include(e => e.ContactFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContactDisplayPropertyFilter), e => string.Format("{0} {1} {2}", e.ContactFk == null || e.ContactFk.Name == null ? "" : e.ContactFk.Name.ToString()
, e.ContactFk == null || e.ContactFk.Surname == null ? "" : e.ContactFk.Surname.ToString()
, e.ContactFk == null || e.ContactFk.IdentifyNo == null ? "" : e.ContactFk.IdentifyNo.ToString()
) == input.ContactDisplayPropertyFilter);

            var query = (from o in filteredUserDetails
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_contactRepository.GetAll() on o.ContactId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetUserDetailForViewDto()
                         {
                             UserDetail = new UserDetailDto
                             {
                                 Id = o.Id
                             },
                             UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ContactDisplayProperty = string.Format("{0} {1} {2}", s2 == null || s2.Name == null ? "" : s2.Name.ToString()
, s2 == null || s2.Surname == null ? "" : s2.Surname.ToString()
, s2 == null || s2.IdentifyNo == null ? "" : s2.IdentifyNo.ToString()
)
                         });

            var userDetailListDtos = await query.ToListAsync();

            return _userDetailsExcelExporter.ExportToFile(userDetailListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_UserDetails)]
        public async Task<PagedResultDto<UserDetailUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UserDetailUserLookupTableDto>();
            foreach (var user in userList)
            {
                lookupTableDtoList.Add(new UserDetailUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user.Name?.ToString()
                });
            }

            return new PagedResultDto<UserDetailUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_UserDetails)]
        public async Task<PagedResultDto<UserDetailContactLookupTableDto>> GetAllContactForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_contactRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => string.Format("{0} {1} {2}", e.Name, e.Surname, e.IdentifyNo).Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var contactList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<UserDetailContactLookupTableDto>();
            foreach (var contact in contactList)
            {
                lookupTableDtoList.Add(new UserDetailContactLookupTableDto
                {
                    Id = contact.Id,
                    DisplayName = string.Format("{0} {1} {2}", contact.Name, contact.Surname, contact.IdentifyNo)
                });
            }

            return new PagedResultDto<UserDetailContactLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}