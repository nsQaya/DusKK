using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Companies;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Corporation;
using TDV.Corporation.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using TDV.Location;
using System.Collections.Generic;
using TDV.Location.Dtos;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Companies)]
    public class CompaniesController : TDVControllerBase
    {
        private readonly ICompaniesAppService _companiesAppService;
        private readonly ICountriesAppService _countriesAppService;
        private readonly ICitiesAppService _cityAppService;
        private readonly IDistrictsAppService _districtsAppService;
        private readonly IQuartersAppService _quartersAppService;

        public CompaniesController(
            ICompaniesAppService companiesAppService, 
            ICitiesAppService cityAppService, 
            IDistrictsAppService districtsAppService, 
            IQuartersAppService quartersAppService,
            ICountriesAppService countriesAppService)
        {
            _companiesAppService = companiesAppService;
            _countriesAppService = countriesAppService;
            _cityAppService = cityAppService;
            _districtsAppService = districtsAppService;
            _quartersAppService = quartersAppService;
        }

        public ActionResult Index()
        {
            var model = new CompaniesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Companies_Create, AppPermissions.Pages_Companies_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            GetCompanyForEditOutput getCompanyForEditOutput;

            if (id.HasValue)
            {
                getCompanyForEditOutput = await _companiesAppService.GetCompanyForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getCompanyForEditOutput = new GetCompanyForEditOutput
                {
                    Company = new CreateOrEditCompanyDto()
                };
            }

            var viewModel = new CreateOrEditCompanyViewModel()
            {
                Company = getCompanyForEditOutput.Company,
                OrganizationUnitDisplayName = getCompanyForEditOutput.OrganizationUnitDisplayName,
                CityDisplayProperty = getCompanyForEditOutput.CityDisplayProperty,
                QuarterName = getCompanyForEditOutput.QuarterName,
                CountryId = getCompanyForEditOutput.CountryId,
                CityId = getCompanyForEditOutput.CityId,
                DistrictId = getCompanyForEditOutput.DistrictId,
                QuarterId= getCompanyForEditOutput.QuarterId,
                CountryList= await _countriesAppService.GetAllCountryForTableDropdown(),
                CityList= new List<CityLookupTableDto>(),
                DistrictList= new List<DistrictLookupTableDto>(),
                QuarterList= new List<QuarterLookupTableDto>(),
            };

            if (getCompanyForEditOutput.CountryId != 0)
            {
                viewModel.CityList = await _cityAppService.GetAllCityForTableDropdown(getCompanyForEditOutput.CountryId);
                viewModel.DistrictList = await _districtsAppService.GetAllDistrictForTableDropdown(getCompanyForEditOutput.CityId);
                viewModel.QuarterList = await _quartersAppService.GetAllQuartersForTableDropdown(getCompanyForEditOutput.DistrictId);
            }

            return View(viewModel);
        }

        public async Task<ActionResult> ViewCompany(int id)
        {
            var getCompanyForViewDto = await _companiesAppService.GetCompanyForView(id);

            var model = new CompanyViewModel()
            {
                Company = getCompanyForViewDto.Company
                ,
                OrganizationUnitDisplayName = getCompanyForViewDto.OrganizationUnitDisplayName

                ,
                CityDisplayProperty = getCompanyForViewDto.CityDisplayProperty

                ,
                QuarterName = getCompanyForViewDto.QuarterName

            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Companies_Create, AppPermissions.Pages_Companies_Edit)]
        public PartialViewResult OrganizationUnitLookupTableModal(long? id, string displayName)
        {
            var viewModel = new CompanyOrganizationUnitLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_CompanyOrganizationUnitLookupTableModal", viewModel);
        }

    }
}