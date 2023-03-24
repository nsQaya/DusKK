using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Regions;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Location;
using TDV.Location.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Regions)]
    public class RegionsController : TDVControllerBase
    {
        private readonly IRegionsAppService _regionsAppService;

        public RegionsController(IRegionsAppService regionsAppService)
        {
            _regionsAppService = regionsAppService;

        }

        public ActionResult Index()
        {
            var model = new RegionsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Regions_Create, AppPermissions.Pages_Regions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetRegionForEditOutput getRegionForEditOutput;

            if (id.HasValue)
            {
                getRegionForEditOutput = await _regionsAppService.GetRegionForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getRegionForEditOutput = new GetRegionForEditOutput
                {
                    Region = new CreateOrEditRegionDto()
                };
            }

            var viewModel = new CreateOrEditRegionModalViewModel()
            {
                Region = getRegionForEditOutput.Region,
                FixedPriceName = getRegionForEditOutput.FixedPriceName,
                RegionFixedPriceList = await _regionsAppService.GetAllFixedPriceForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewRegionModal(int id)
        {
            var getRegionForViewDto = await _regionsAppService.GetRegionForView(id);

            var model = new RegionViewModel()
            {
                Region = getRegionForViewDto.Region
                ,
                FixedPriceName = getRegionForViewDto.FixedPriceName

            };

            return PartialView("_ViewRegionModal", model);
        }

    }
}