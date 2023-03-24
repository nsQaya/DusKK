using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Stoks;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Kalite;
using TDV.Kalite.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Stoks)]
    public class StoksController : TDVControllerBase
    {
        private readonly IStoksAppService _stoksAppService;

        public StoksController(IStoksAppService stoksAppService)
        {
            _stoksAppService = stoksAppService;

        }

        public ActionResult Index()
        {
            var model = new StoksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Stoks_Create, AppPermissions.Pages_Stoks_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetStokForEditOutput getStokForEditOutput;

            if (id.HasValue)
            {
                getStokForEditOutput = await _stoksAppService.GetStokForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getStokForEditOutput = new GetStokForEditOutput
                {
                    Stok = new CreateOrEditStokDto()
                };
            }

            var viewModel = new CreateOrEditStokModalViewModel()
            {
                Stok = getStokForEditOutput.Stok,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewStokModal(int id)
        {
            var getStokForViewDto = await _stoksAppService.GetStokForView(id);

            var model = new StokViewModel()
            {
                Stok = getStokForViewDto.Stok
            };

            return PartialView("_ViewStokModal", model);
        }

    }
}