using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Vehicles;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Corporation;
using TDV.Corporation.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Vehicles)]
    public class VehiclesController : TDVControllerBase
    {
        private readonly IVehiclesAppService _vehiclesAppService;

        public VehiclesController(IVehiclesAppService vehiclesAppService)
        {
            _vehiclesAppService = vehiclesAppService;

        }

        public ActionResult Index()
        {
            var model = new VehiclesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Vehicles_Create, AppPermissions.Pages_Vehicles_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetVehicleForEditOutput getVehicleForEditOutput;

            if (id.HasValue)
            {
                getVehicleForEditOutput = await _vehiclesAppService.GetVehicleForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getVehicleForEditOutput = new GetVehicleForEditOutput
                {
                    Vehicle = new CreateOrEditVehicleDto()
                };
                getVehicleForEditOutput.Vehicle.EndExaminationDate = DateTime.Now;
                getVehicleForEditOutput.Vehicle.EndInsuranceDate = DateTime.Now;
                getVehicleForEditOutput.Vehicle.EndGuarantyDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditVehicleModalViewModel()
            {
                Vehicle = getVehicleForEditOutput.Vehicle,
                CompanyDisplayProperty = getVehicleForEditOutput.CompanyDisplayProperty,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewVehicleModal(int id)
        {
            var getVehicleForViewDto = await _vehiclesAppService.GetVehicleForView(id);

            var model = new VehicleViewModel()
            {
                Vehicle = getVehicleForViewDto.Vehicle
                ,
                CompanyDisplayProperty = getVehicleForViewDto.CompanyDisplayProperty

            };

            return PartialView("_ViewVehicleModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Vehicles_Create, AppPermissions.Pages_Vehicles_Edit)]
        public PartialViewResult CompanyLookupTableModal(int? id, string displayName)
        {
            var viewModel = new VehicleCompanyLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_VehicleCompanyLookupTableModal", viewModel);
        }

    }
}