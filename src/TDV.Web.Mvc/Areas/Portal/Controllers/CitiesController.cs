using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Cities;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Location;
using TDV.Location.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Cities)]
    public class CitiesController : TDVControllerBase
    {
        private readonly ICountriesAppService _countriesAppService;
        private readonly ICitiesAppService _citiesAppService;

        public CitiesController(ICitiesAppService citiesAppService, ICountriesAppService countriesAppService)
        {
            _citiesAppService = citiesAppService;
            _countriesAppService = countriesAppService;
        }

        public ActionResult Index()
        {
            var model = new CitiesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Cities_Create, AppPermissions.Pages_Cities_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCityForEditOutput getCityForEditOutput;

            if (id.HasValue)
            {
                getCityForEditOutput = await _citiesAppService.GetCityForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getCityForEditOutput = new GetCityForEditOutput
                {
                    City = new CreateOrEditCityDto()
                };
            }

            var viewModel = new CreateOrEditCityModalViewModel()
            {
                City = getCityForEditOutput.City,
                CountryDisplayProperty = getCityForEditOutput.CountryDisplayProperty,
                CityCountryList = await _countriesAppService.GetAllCountryForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCityModal(int id)
        {
            var getCityForViewDto = await _citiesAppService.GetCityForView(id);

            var model = new CityViewModel()
            {
                City = getCityForViewDto.City
                ,
                CountryDisplayProperty = getCityForViewDto.CountryDisplayProperty

            };

            return PartialView("_ViewCityModal", model);
        }

    }
}