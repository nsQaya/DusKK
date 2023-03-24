using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Airports;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Flight;
using TDV.Flight.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using TDV.Location;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Airports)]
    public class AirportsController : TDVControllerBase
    {
        private readonly IAirportsAppService _airportsAppService;
        private readonly ICountriesAppService _countriesAppService;
        private readonly ICitiesAppService _citiesAppService;

        public AirportsController(IAirportsAppService airportsAppService, ICountriesAppService countriesAppService, ICitiesAppService citiesAppService)
        {
            _airportsAppService = airportsAppService;
            _countriesAppService = countriesAppService;
            _citiesAppService = citiesAppService;
        }

        public ActionResult Index()
        {
            var model = new AirportsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Airports_Create, AppPermissions.Pages_Airports_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetAirportForEditOutput getAirportForEditOutput;

            if (id.HasValue)
            {
                getAirportForEditOutput = await _airportsAppService.GetAirportForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getAirportForEditOutput = new GetAirportForEditOutput
                {
                    Airport = new CreateOrEditAirportDto()
                };
            }

            var viewModel = new CreateOrEditAirportModalViewModel()
            {
                Airport = getAirportForEditOutput.Airport,
                CountryDisplayProperty = getAirportForEditOutput.CountryDisplayProperty,
                CityDisplayProperty = getAirportForEditOutput.CityDisplayProperty,
                AirportCountryList = await _countriesAppService.GetAllCountryForTableDropdown(),
                AirportCityList = getAirportForEditOutput.Airport.CountryId!=0 ?  await _citiesAppService.GetAllCityForTableDropdown(getAirportForEditOutput.Airport.CountryId) : null,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewAirportModal(int id)
        {
            var getAirportForViewDto = await _airportsAppService.GetAirportForView(id);

            var model = new AirportViewModel()
            {
                Airport = getAirportForViewDto.Airport
                ,
                CountryDisplayProperty = getAirportForViewDto.CountryDisplayProperty

                ,
                CityDisplayProperty = getAirportForViewDto.CityDisplayProperty

            };

            return PartialView("_ViewAirportModal", model);
        }

    }
}