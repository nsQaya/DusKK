using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Contacts;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Communication;
using TDV.Communication.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Contacts)]
    public class ContactsController : TDVControllerBase
    {
        private readonly IContactsAppService _contactsAppService;

        public ContactsController(IContactsAppService contactsAppService)
        {
            _contactsAppService = contactsAppService;

        }

        public ActionResult Index()
        {
            var model = new ContactsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Contacts_Create, AppPermissions.Pages_Contacts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetContactForEditOutput getContactForEditOutput;

            if (id.HasValue)
            {
                getContactForEditOutput = await _contactsAppService.GetContactForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getContactForEditOutput = new GetContactForEditOutput
                {
                    Contact = new CreateOrEditContactDto()
                };
            }

            var viewModel = new CreateOrEditContactModalViewModel()
            {
                Contact = getContactForEditOutput.Contact,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewContactModal(int id)
        {
            var getContactForViewDto = await _contactsAppService.GetContactForView(id);

            var model = new ContactViewModel()
            {
                Contact = getContactForViewDto.Contact
            };

            return PartialView("_ViewContactModal", model);
        }

    }
}