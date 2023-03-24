using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Districts;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Location;
using TDV.Location.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Districts)]
    public class DistrictsController : TDVControllerBase
    {
        private readonly ICountriesAppService _countriesAppService;
        private readonly IDistrictsAppService _districtsAppService;
        private readonly ICitiesAppService _citiesAppService;
        private readonly IRegionsAppService _regionsAppService;

        public DistrictsController(ICountriesAppService countriesAppService, IDistrictsAppService districtsAppService, ICitiesAppService citiesAppService, IRegionsAppService regionsAppService)
        {
            _countriesAppService = countriesAppService;
            _districtsAppService = districtsAppService;
            _citiesAppService = citiesAppService;
            _regionsAppService = regionsAppService;
        }

        public ActionResult Index()
        {
            var model = new DistrictsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Districts_Create, AppPermissions.Pages_Districts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDistrictForEditOutput getDistrictForEditOutput;

            if (id.HasValue)
            {
                getDistrictForEditOutput = await _districtsAppService.GetDistrictForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getDistrictForEditOutput = new GetDistrictForEditOutput
                {
                    District = new CreateOrEditDistrictDto()
                };
            }

            var viewModel = new CreateOrEditDistrictModalViewModel()
            {
                District = getDistrictForEditOutput.District,
                CityDisplayProperty = getDistrictForEditOutput.CityDisplayProperty,
                RegionName = getDistrictForEditOutput.RegionName,
                CountryList= await _countriesAppService.GetAllCountryForTableDropdown(), // İL İÇİN ÖNCE ÜLKENİN SEÇİLMESİ GEREKLİ !
                DistrictCityList = getDistrictForEditOutput.District.CityId!=0 ? await _citiesAppService.GetAllCityForTableDropdown(getDistrictForEditOutput.District.CountryId) : null,
                DistrictRegionList = await _regionsAppService.GetAllRegionForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDistrictModal(int id)
        {
            var getDistrictForViewDto = await _districtsAppService.GetDistrictForView(id);

            var model = new DistrictViewModel()
            {
                District = getDistrictForViewDto.District
                ,
                CityDisplayProperty = getDistrictForViewDto.CityDisplayProperty

                ,
                RegionName = getDistrictForViewDto.RegionName

            };

            return PartialView("_ViewDistrictModal", model);
        }

    }
}