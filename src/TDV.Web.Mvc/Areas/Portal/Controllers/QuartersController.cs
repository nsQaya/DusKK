using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Quarters;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Location;
using TDV.Location.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Quarters)]
    public class QuartersController : TDVControllerBase
    {
        private readonly ICountriesAppService _countriesService;
        private readonly ICitiesAppService _citiesAppService;
        private readonly IDistrictsAppService _districtsAppService;
        private readonly IQuartersAppService _quartersAppService;


        public QuartersController(ICountriesAppService countriesService, IQuartersAppService quartersAppService, ICitiesAppService citiesAppService, IDistrictsAppService districtsAppService)
        {
            _countriesService = countriesService;
            _quartersAppService = quartersAppService;
            _citiesAppService = citiesAppService;
            _districtsAppService = districtsAppService;
        }

        public ActionResult Index()
        {
            var model = new QuartersViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Quarters_Create, AppPermissions.Pages_Quarters_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetQuarterForEditOutput getQuarterForEditOutput;

            if (id.HasValue)
            {
                getQuarterForEditOutput = await _quartersAppService.GetQuarterForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getQuarterForEditOutput = new GetQuarterForEditOutput
                {
                    Quarter = new CreateOrEditQuarterDto()
                };
            }

            var viewModel = new CreateOrEditQuarterModalViewModel()
            {
                Quarter = getQuarterForEditOutput.Quarter,
                DistrictName = getQuarterForEditOutput.DistrictName,
                CountryList= await _countriesService.GetAllCountryForTableDropdown(),
                CityList = getQuarterForEditOutput.Quarter.CityId != 0 ? await _citiesAppService.GetAllCityForTableDropdown(getQuarterForEditOutput.Quarter.CountryId) : null,
                QuarterDistrictList = getQuarterForEditOutput.Quarter.DistrictId != 0 ? await _districtsAppService.GetAllDistrictForTableDropdown(getQuarterForEditOutput.Quarter.CityId) : null,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewQuarterModal(int id)
        {
            var getQuarterForViewDto = await _quartersAppService.GetQuarterForView(id);

            var model = new QuarterViewModel()
            {
                Quarter = getQuarterForViewDto.Quarter
                ,
                DistrictName = getQuarterForViewDto.DistrictName

            };

            return PartialView("_ViewQuarterModal", model);
        }

    }
}