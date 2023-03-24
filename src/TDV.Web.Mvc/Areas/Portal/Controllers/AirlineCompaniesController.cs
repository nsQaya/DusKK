using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.AirlineCompanies;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Flight;
using TDV.Flight.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_AirlineCompanies)]
    public class AirlineCompaniesController : TDVControllerBase
    {
        private readonly IAirlineCompaniesAppService _airlineCompaniesAppService;

        public AirlineCompaniesController(IAirlineCompaniesAppService airlineCompaniesAppService)
        {
            _airlineCompaniesAppService = airlineCompaniesAppService;

        }

        public ActionResult Index()
        {
            var model = new AirlineCompaniesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_AirlineCompanies_Create, AppPermissions.Pages_AirlineCompanies_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAirlineCompanyForEditOutput getAirlineCompanyForEditOutput;

            if (id.HasValue)
            {
                getAirlineCompanyForEditOutput = await _airlineCompaniesAppService.GetAirlineCompanyForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAirlineCompanyForEditOutput = new GetAirlineCompanyForEditOutput
                {
                    AirlineCompany = new CreateOrEditAirlineCompanyDto()
                };
            }

            var viewModel = new CreateOrEditAirlineCompanyModalViewModel()
            {
                AirlineCompany = getAirlineCompanyForEditOutput.AirlineCompany,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAirlineCompanyModal(int id)
        {
            var getAirlineCompanyForViewDto = await _airlineCompaniesAppService.GetAirlineCompanyForView(id);

            var model = new AirlineCompanyViewModel()
            {
                AirlineCompany = getAirlineCompanyForViewDto.AirlineCompany
            };

            return PartialView("_ViewAirlineCompanyModal", model);
        }

    }
}