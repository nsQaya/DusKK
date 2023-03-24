using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.FuneralAddreses;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Burial;
using TDV.Burial.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_FuneralAddreses)]
    public class FuneralAddresesController : TDVControllerBase
    {
        private readonly IFuneralAddresesAppService _funeralAddresesAppService;

        public FuneralAddresesController(IFuneralAddresesAppService funeralAddresesAppService)
        {
            _funeralAddresesAppService = funeralAddresesAppService;

        }

        public ActionResult Index()
        {
            var model = new FuneralAddresesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralAddreses_Create, AppPermissions.Pages_FuneralAddreses_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetFuneralAddresForEditOutput getFuneralAddresForEditOutput;

            if (id.HasValue)
            {
                getFuneralAddresForEditOutput = await _funeralAddresesAppService.GetFuneralAddresForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFuneralAddresForEditOutput = new GetFuneralAddresForEditOutput
                {
                    FuneralAddres = new CreateOrEditFuneralAddresDto()
                };
            }

            var viewModel = new CreateOrEditFuneralAddresModalViewModel()
            {
                FuneralAddres = getFuneralAddresForEditOutput.FuneralAddres,
                FuneralDisplayProperty = getFuneralAddresForEditOutput.FuneralDisplayProperty,
                QuarterName = getFuneralAddresForEditOutput.QuarterName,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFuneralAddresModal(int id)
        {
            var getFuneralAddresForViewDto = await _funeralAddresesAppService.GetFuneralAddresForView(id);

            var model = new FuneralAddresViewModel()
            {
                FuneralAddres = getFuneralAddresForViewDto.FuneralAddres
                ,
                FuneralDisplayProperty = getFuneralAddresForViewDto.FuneralDisplayProperty

                ,
                QuarterName = getFuneralAddresForViewDto.QuarterName

            };

            return PartialView("_ViewFuneralAddresModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralAddreses_Create, AppPermissions.Pages_FuneralAddreses_Edit)]
        public PartialViewResult FuneralLookupTableModal(int? id, string displayName)
        {
            var viewModel = new FuneralAddresFuneralLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralAddresFuneralLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_FuneralAddreses_Create, AppPermissions.Pages_FuneralAddreses_Edit)]
        public PartialViewResult QuarterLookupTableModal(int? id, string displayName)
        {
            var viewModel = new FuneralAddresQuarterLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralAddresQuarterLookupTableModal", viewModel);
        }

    }
}