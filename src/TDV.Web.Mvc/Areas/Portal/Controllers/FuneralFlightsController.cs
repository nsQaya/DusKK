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
    public class FuneralFlightsController : TDVControllerBase
    {
        private readonly IFuneralFlightsAppService _funeralFlightsAppService;

        public FuneralFlightsController(IFuneralFlightsAppService funeralFlightsAppService)
        {
            _funeralFlightsAppService = funeralFlightsAppService;

        }

        public ActionResult Index()
        {
            var model = new FuneralFlightsViewModel
            {
                FilterText = ""
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

            var viewModel = new CreateOrEditFuneralFlightModalViewModel()
            {
                FuneralFlight = getFuneralFlightForEditOutput.FuneralFlight,
                FuneralName = getFuneralFlightForEditOutput.FuneralName,
                AirlineCompanyCode = getFuneralFlightForEditOutput.AirlineCompanyCode,
                AirportName = getFuneralFlightForEditOutput.AirportName,
                AirportName2 = getFuneralFlightForEditOutput.AirportName2,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFuneralFlightModal(int id)
        {
            var getFuneralFlightForViewDto = await _funeralFlightsAppService.GetFuneralFlightForView(id);

            var model = new FuneralFlightViewModel()
            {
                FuneralFlight = getFuneralFlightForViewDto.FuneralFlight
                ,
                FuneralName = getFuneralFlightForViewDto.FuneralName

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
        public PartialViewResult FuneralLookupTableModal(int? id, string displayName)
        {
            var viewModel = new FuneralFlightFuneralLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralFlightFuneralLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_FuneralFlights_Create, AppPermissions.Pages_FuneralFlights_Edit)]
        public PartialViewResult AirlineCompanyLookupTableModal(int? id, string displayName)
        {
            var viewModel = new FuneralFlightAirlineCompanyLookupTableViewModel()
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
            var viewModel = new FuneralFlightAirportLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralFlightAirportLookupTableModal", viewModel);
        }

    }
}