using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.CompanyContacts;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Corporation;
using TDV.Corporation.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_CompanyContacts)]
    public class CompanyContactsController : TDVControllerBase
    {
        private readonly ICompanyContactsAppService _companyContactsAppService;

        public CompanyContactsController(ICompanyContactsAppService companyContactsAppService)
        {
            _companyContactsAppService = companyContactsAppService;

        }

        public ActionResult Index()
        {
            var model = new CompanyContactsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CompanyContacts_Create, AppPermissions.Pages_CompanyContacts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCompanyContactForEditOutput getCompanyContactForEditOutput;

            if (id.HasValue)
            {
                getCompanyContactForEditOutput = await _companyContactsAppService.GetCompanyContactForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getCompanyContactForEditOutput = new GetCompanyContactForEditOutput
                {
                    CompanyContact = new CreateOrEditCompanyContactDto()
                };
            }

            var viewModel = new CreateOrEditCompanyContactModalViewModel()
            {
                CompanyContact = getCompanyContactForEditOutput.CompanyContact,
                CompanyDisplayProperty = getCompanyContactForEditOutput.CompanyDisplayProperty,
                ContactName = getCompanyContactForEditOutput.ContactName,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCompanyContactModal(int id)
        {
            var getCompanyContactForViewDto = await _companyContactsAppService.GetCompanyContactForView(id);

            var model = new CompanyContactViewModel()
            {
                CompanyContact = getCompanyContactForViewDto.CompanyContact
                ,
                CompanyDisplayProperty = getCompanyContactForViewDto.CompanyDisplayProperty

                ,
                ContactName = getCompanyContactForViewDto.ContactName

            };

            return PartialView("_ViewCompanyContactModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CompanyContacts_Create, AppPermissions.Pages_CompanyContacts_Edit)]
        public PartialViewResult CompanyLookupTableModal(int? id, string displayName)
        {
            var viewModel = new CompanyContactCompanyLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CompanyContactCompanyLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_CompanyContacts_Create, AppPermissions.Pages_CompanyContacts_Edit)]
        public PartialViewResult ContactLookupTableModal(int? id, string displayName)
        {
            var viewModel = new CompanyContactContactLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CompanyContactContactLookupTableModal", viewModel);
        }

    }
}