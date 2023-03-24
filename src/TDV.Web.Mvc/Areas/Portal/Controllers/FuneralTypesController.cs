using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.FuneralTypes;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Burial;
using TDV.Burial.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_FuneralTypes)]
    public class FuneralTypesController : TDVControllerBase
    {
        private readonly IFuneralTypesAppService _funeralTypesAppService;

        public FuneralTypesController(IFuneralTypesAppService funeralTypesAppService)
        {
            _funeralTypesAppService = funeralTypesAppService;

        }

        public ActionResult Index()
        {
            var model = new FuneralTypesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralTypes_Create, AppPermissions.Pages_FuneralTypes_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetFuneralTypeForEditOutput getFuneralTypeForEditOutput;

            if (id.HasValue)
            {
                getFuneralTypeForEditOutput = await _funeralTypesAppService.GetFuneralTypeForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFuneralTypeForEditOutput = new GetFuneralTypeForEditOutput
                {
                    FuneralType = new CreateOrEditFuneralTypeDto()
                };
            }

            var viewModel = new CreateOrEditFuneralTypeModalViewModel()
            {
                FuneralType = getFuneralTypeForEditOutput.FuneralType,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFuneralTypeModal(int id)
        {
            var getFuneralTypeForViewDto = await _funeralTypesAppService.GetFuneralTypeForView(id);

            var model = new FuneralTypeViewModel()
            {
                FuneralType = getFuneralTypeForViewDto.FuneralType
            };

            return PartialView("_ViewFuneralTypeModal", model);
        }

    }
}