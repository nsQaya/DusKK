using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.FuneralFlights;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Burial;
using TDV.Burial.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_FuneralFlights)]
    public class MasterDetailChild_Funeral_FuneralFlightsController : TDVControllerBase
    {
        private readonly IFuneralFlightsAppService _funeralFlightsAppService;

        public MasterDetailChild_Funeral_FuneralFlightsController(IFuneralFlightsAppService funeralFlightsAppService)
        {
            _funeralFlightsAppService = funeralFlightsAppService;
        }

        public ActionResult Index(int funeralId)
        {
            var model = new MasterDetailChild_Funeral_FuneralFlightsViewModel
            {
                FilterText = "",
                FuneralId = funeralId
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralFlights_Create, AppPermissions.Pages_FuneralFlights_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetFuneralFlightForEditOutput getFuneralFlightForEditOutput;

            if (id.HasValue)
            {
                getFuneralFlightForEditOutput = await _funeralFlightsAppService.GetFuneralFlightForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFuneralFlightForEditOutput = new GetFuneralFlightForEditOutput
                {
                    FuneralFlight = new CreateOrEditFuneralFlightDto()
                };
                getFuneralFlightForEditOutput.FuneralFlight.LiftOffDate = DateTime.Now;
                getFuneralFlightForEditOutput.FuneralFlight.LandingDate = DateTime.Now;
            }

            var viewModel = new MasterDetailChild_Funeral_CreateOrEditFuneralFlightModalViewModel()
            {
                FuneralFlight = getFuneralFlightForEditOutput.FuneralFlight,
                AirlineCompanyCode = getFuneralFlightForEditOutput.AirlineCompanyCode,
                AirportName = getFuneralFlightForEditOutput.AirportName,
                AirportName2 = getFuneralFlightForEditOutput.AirportName2,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFuneralFlightModal(int id)
        {
            var getFuneralFlightForViewDto = await _funeralFlightsAppService.GetFuneralFlightForView(id);

            var model = new MasterDetailChild_Funeral_FuneralFlightViewModel()
            {
                FuneralFlight = getFuneralFlightForViewDto.FuneralFlight
                ,
                AirlineCompanyCode = getFuneralFlightForViewDto.AirlineCompanyCode

                ,
                AirportName = getFuneralFlightForViewDto.AirportName

                ,
                AirportName2 = getFuneralFlightForViewDto.AirportName2

            };

            return PartialView("_ViewFuneralFlightModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralFlights_Create, AppPermissions.Pages_FuneralFlights_Edit)]
        public PartialViewResult AirlineCompanyLookupTableModal(int? id, string displayName)
        {
            var viewModel = new MasterDetailChild_Funeral_FuneralFlightAirlineCompanyLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralFlightAirlineCompanyLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_FuneralFlights_Create, AppPermissions.Pages_FuneralFlights_Edit)]
        public PartialViewResult AirportLookupTableModal(int? id, string displayName)
        {
            var viewModel = new MasterDetailChild_Funeral_FuneralFlightAirportLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralFlightAirportLookupTableModal", viewModel);
        }

    }
}