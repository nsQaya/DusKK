using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.FuneralPackages;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Burial;
using TDV.Burial.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_FuneralPackages)]
    public class FuneralPackagesController : TDVControllerBase
    {
        private readonly IFuneralPackagesAppService _funeralPackagesAppService;

        public FuneralPackagesController(IFuneralPackagesAppService funeralPackagesAppService)
        {
            _funeralPackagesAppService = funeralPackagesAppService;

        }

        public ActionResult Index()
        {
            var model = new FuneralPackagesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralPackages_Create, AppPermissions.Pages_FuneralPackages_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetFuneralPackageForEditOutput getFuneralPackageForEditOutput;

            if (id.HasValue)
            {
                getFuneralPackageForEditOutput = await _funeralPackagesAppService.GetFuneralPackageForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFuneralPackageForEditOutput = new GetFuneralPackageForEditOutput
                {
                    FuneralPackage = new CreateOrEditFuneralPackageDto()
                };
            }

            var viewModel = new CreateOrEditFuneralPackageModalViewModel()
            {
                FuneralPackage = getFuneralPackageForEditOutput.FuneralPackage,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFuneralPackageModal(int id)
        {
            var getFuneralPackageForViewDto = await _funeralPackagesAppService.GetFuneralPackageForView(id);

            var model = new FuneralPackageViewModel()
            {
                FuneralPackage = getFuneralPackageForViewDto.FuneralPackage
            };

            return PartialView("_ViewFuneralPackageModal", model);
        }

    }
}