using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.ContactDetails;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Communication;
using TDV.Communication.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_ContactDetails)]
    public class ContactDetailsController : TDVControllerBase
    {
        private readonly IContactDetailsAppService _contactDetailsAppService;

        public ContactDetailsController(IContactDetailsAppService contactDetailsAppService)
        {
            _contactDetailsAppService = contactDetailsAppService;

        }

        public ActionResult Index()
        {
            var model = new ContactDetailsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ContactDetails_Create, AppPermissions.Pages_ContactDetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetContactDetailForEditOutput getContactDetailForEditOutput;

            if (id.HasValue)
            {
                getContactDetailForEditOutput = await _contactDetailsAppService.GetContactDetailForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getContactDetailForEditOutput = new GetContactDetailForEditOutput
                {
                    ContactDetail = new CreateOrEditContactDetailDto()
                };
            }

            var viewModel = new CreateOrEditContactDetailModalViewModel()
            {
                ContactDetail = getContactDetailForEditOutput.ContactDetail,
                ContactIdentifyNo = getContactDetailForEditOutput.ContactIdentifyNo,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewContactDetailModal(int id)
        {
            var getContactDetailForViewDto = await _contactDetailsAppService.GetContactDetailForView(id);

            var model = new ContactDetailViewModel()
            {
                ContactDetail = getContactDetailForViewDto.ContactDetail
                ,
                ContactIdentifyNo = getContactDetailForViewDto.ContactIdentifyNo

            };

            return PartialView("_ViewContactDetailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ContactDetails_Create, AppPermissions.Pages_ContactDetails_Edit)]
        public PartialViewResult ContactLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ContactDetailContactLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ContactDetailContactLookupTableModal", viewModel);
        }

    }
}