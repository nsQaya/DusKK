using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.UserDetails;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Authorization;
using TDV.Authorization.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_UserDetails)]
    public class UserDetailsController : TDVControllerBase
    {
        private readonly IUserDetailsAppService _userDetailsAppService;

        public UserDetailsController(IUserDetailsAppService userDetailsAppService)
        {
            _userDetailsAppService = userDetailsAppService;

        }

        public ActionResult Index()
        {
            var model = new UserDetailsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserDetails_Create, AppPermissions.Pages_UserDetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetUserDetailForEditOutput getUserDetailForEditOutput;

            if (id.HasValue)
            {
                getUserDetailForEditOutput = await _userDetailsAppService.GetUserDetailForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getUserDetailForEditOutput = new GetUserDetailForEditOutput
                {
                    UserDetail = new CreateOrEditUserDetailDto()
                };
            }

            var viewModel = new CreateOrEditUserDetailModalViewModel()
            {
                UserDetail = getUserDetailForEditOutput.UserDetail,
                UserName = getUserDetailForEditOutput.UserName,
                ContactDisplayProperty = getUserDetailForEditOutput.ContactDisplayProperty,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewUserDetailModal(int id)
        {
            var getUserDetailForViewDto = await _userDetailsAppService.GetUserDetailForView(id);

            var model = new UserDetailViewModel()
            {
                UserDetail = getUserDetailForViewDto.UserDetail
                ,
                UserName = getUserDetailForViewDto.UserName

                ,
                ContactDisplayProperty = getUserDetailForViewDto.ContactDisplayProperty

            };

            return PartialView("_ViewUserDetailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserDetails_Create, AppPermissions.Pages_UserDetails_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new UserDetailUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserDetailUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_UserDetails_Create, AppPermissions.Pages_UserDetails_Edit)]
        public PartialViewResult ContactLookupTableModal(int? id, string displayName)
        {
            var viewModel = new UserDetailContactLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserDetailContactLookupTableModal", viewModel);
        }

    }
}