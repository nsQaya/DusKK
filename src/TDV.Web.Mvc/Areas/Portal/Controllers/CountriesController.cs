using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Countries;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Location;
using TDV.Location.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesController : TDVControllerBase
    {
        private readonly ICountriesAppService _countriesAppService;

        public CountriesController(ICountriesAppService countriesAppService)
        {
            _countriesAppService = countriesAppService;

        }

        public ActionResult Index()
        {
            var model = new CountriesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Countries_Create, AppPermissions.Pages_Countries_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCountryForEditOutput getCountryForEditOutput;

            if (id.HasValue)
            {
                getCountryForEditOutput = await _countriesAppService.GetCountryForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getCountryForEditOutput = new GetCountryForEditOutput
                {
                    Country = new CreateOrEditCountryDto()
                };
            }

            var viewModel = new CreateOrEditCountryModalViewModel()
            {
                Country = getCountryForEditOutput.Country,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCountryModal(int id)
        {
            var getCountryForViewDto = await _countriesAppService.GetCountryForView(id);

            var model = new CountryViewModel()
            {
                Country = getCountryForViewDto.Country
            };

            return PartialView("_ViewCountryModal", model);
        }

    }
}