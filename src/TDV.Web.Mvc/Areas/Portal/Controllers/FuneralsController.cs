using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Funerals;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Burial;
using TDV.Burial.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using TDV.Location;
using System.Collections.Generic;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Funerals)]
    public class FuneralsController : TDVControllerBase
    {
        private readonly IFuneralsAppService _funeralsAppService;
        private readonly ICountriesAppService _countriesAppService;
        private readonly IQuartersAppService _quartersAppService;
        private readonly ICitiesAppService _citiesAppService;
        private readonly IDistrictsAppService _districtsAppService;
        public FuneralsController(
            IFuneralsAppService funeralsAppService,
            IQuartersAppService quartersAppService,
            ICitiesAppService citiesAppService,
            IDistrictsAppService districtsAppService,
            ICountriesAppService countriesAppService
            )
        {
            _funeralsAppService = funeralsAppService;
            _quartersAppService = quartersAppService;
            _citiesAppService = citiesAppService;
            _districtsAppService = districtsAppService;
            _countriesAppService = countriesAppService;
        }

        public ActionResult Index()
        {
            var model = new FuneralsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public ActionResult Assignment()
        {
            var model = new FuneralsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public ActionResult DriverAssignment()
        {
            var model = new FuneralsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public ActionResult Driver()
        {
            var model = new FuneralsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Funerals_Create, AppPermissions.Pages_Funerals_Edit)]
        public async Task<ActionResult> CreateOrEdit(int? id)
        {
            GetFuneralForEditOutput getFuneralForEditOutput;

            if (id.HasValue)
            {
                getFuneralForEditOutput = await _funeralsAppService.GetFuneralForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFuneralForEditOutput = new GetFuneralForEditOutput
                {
                    Funeral = new CreateOrEditFuneralDto()
                };
                getFuneralForEditOutput.Funeral.OperationDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditFuneralViewModel()
            {
                Funeral = getFuneralForEditOutput.Funeral,
                FuneralTypeDescription = getFuneralForEditOutput.FuneralTypeDescription,
                ContactDisplayProperty = getFuneralForEditOutput.ContactDisplayProperty,
                OwnerOrganizationUnitDisplayName = getFuneralForEditOutput.OwnerOrganizationUnitDisplayName,
                GiverOrganizationUnitDisplayName = getFuneralForEditOutput.GiverOrganizationUnitDisplayName,
                ContractorOrganizationUnitDisplayName = getFuneralForEditOutput.ContractorOrganizationUnitDisplayName,
                UserName = getFuneralForEditOutput.UserName,
                FuneralPackageCode = getFuneralForEditOutput.FuneralPackageCode,
                ContractFormule = getFuneralForEditOutput.ContractFormule,
                VehiclePlate = getFuneralForEditOutput.VehiclePlate,
                FuneralFuneralTypeList = await _funeralsAppService.GetAllFuneralTypeForTableDropdown(),
                FuneralOrganizationUnitList = await _funeralsAppService.GetAllOrganizationUnitForTableDropdown(),
                FuneralUserList = await _funeralsAppService.GetAllUserForTableDropdown(),
                FuneralFuneralPackageList = await _funeralsAppService.GetAllFuneralPackageForTableDropdown(),
                FuneralContractList = await _funeralsAppService.GetAllContractForTableDropdown(),
                FuneralVehicleList = await _funeralsAppService.GetAllVehicleForTableDropdown(),
                RegionDisplayProperty = getFuneralForEditOutput.RegionDisplayProperty,
                LandingAirportDisplayProperty = getFuneralForEditOutput.LandingAirportDisplayProperty,
                LiftOffAirportDisplayProperty = getFuneralForEditOutput.AirlineDisplayProperty,
                AirlineDisplayProperty = getFuneralForEditOutput.AirlineDisplayProperty,
            };
            /*
            if (getFuneralForEditOutput?.Funeral?.Address?.QuarterId != null)
            {
                viewModel.CityList = await _districtsAppService.GetAllCityForTableDropdown(getFuneralForEditOutput.CountryId);
                viewModel.DistrictList = await _quartersAppService.GetAllDistrictForTableDropdown(getFuneralForEditOutput.CityId);
                viewModel.QuarterList = await _quartersAppService.GetAllQuartersForTableDropdown(getFuneralForEditOutput.DistrictId);
            }
            */

            return View(viewModel);
        }

        public async Task<ActionResult> ViewFuneral(int id)
        {
            var getFuneralForViewDto = await _funeralsAppService.GetFuneralForView(id);

            var model = new FuneralViewModel()
            {
                Funeral = getFuneralForViewDto.Funeral,
                FuneralTypeDescription = getFuneralForViewDto.FuneralTypeDescription,
                ContactDisplayProperty = getFuneralForViewDto.ContactDisplayProperty,
                OwnerOrganizationUnitDisplayName = getFuneralForViewDto.OwnerOrganizationUnitDisplayName,
                GiverOrganizationUnitDisplayName = getFuneralForViewDto.GiverOrganizationUnitDisplayName,
                ContractorOrganizationUnitDisplayName = getFuneralForViewDto.ContractorOrganizationUnitDisplayName,
                UserName = getFuneralForViewDto.UserName,
                FuneralPackageCode = getFuneralForViewDto.FuneralPackageCode,
                CityDisplayName= getFuneralForViewDto.CityDisplayName,
                CountryDisplayName= getFuneralForViewDto.CountryDisplayName,
                DistrictDisplayName= getFuneralForViewDto.DistrictDisplayName,
                QuarterDisplayName= getFuneralForViewDto.QuarterDisplayName,
                RegionDisplayName= getFuneralForViewDto.RegionDisplayName,
                AirlineCompanyDisplayName= getFuneralForViewDto.AirlineCompanyDisplayName,
                LandingAirportDisplayName= getFuneralForViewDto.LandingAirportDisplayName,
                LiftOffAirportDisplayName= getFuneralForViewDto.LiftOffAirportDisplayName
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Funerals_Funeral_Assignment)]
        public async Task<ActionResult> FuneralAssignmentModal(List<int> ids)
        {
            var model = new FuneralAssignmentModel()
            {
                CompanyList = await _funeralsAppService.GetCompaniesForPackage(ids)
            };

            return PartialView("_FuneralAssignmentModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Funerals_Driver_Assignment)]
        public async Task<ActionResult> DriverAssignmentModal(int packageId)
        {
            var model = new DriverAssignmentModel()
            {
               VehicleList= await _funeralsAppService.GetVehiclesForPackage(packageId),
               EmployeerList= await _funeralsAppService.GetEmployeesForPackage(packageId)
            };

            return PartialView("_DriverAssignmentModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Funerals_Create, AppPermissions.Pages_Funerals_Edit)]
        public PartialViewResult ContactLookupTableModal(int? id, string displayName)
        {
            var viewModel = new FuneralContactLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralContactLookupTableModal", viewModel);
        }

    }
}