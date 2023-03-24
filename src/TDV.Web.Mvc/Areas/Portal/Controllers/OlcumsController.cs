using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Olcums;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Kalite;
using TDV.Kalite.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Olcums)]
    public class OlcumsController : TDVControllerBase
    {
        private readonly IOlcumsAppService _olcumsAppService;

        public OlcumsController(IOlcumsAppService olcumsAppService)
        {
            _olcumsAppService = olcumsAppService;

        }

        public ActionResult Index()
        {
            var model = new OlcumsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Olcums_Create, AppPermissions.Pages_Olcums_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetOlcumForEditOutput getOlcumForEditOutput;

            if (id.HasValue)
            {
                getOlcumForEditOutput = await _olcumsAppService.GetOlcumForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getOlcumForEditOutput = new GetOlcumForEditOutput
                {
                    Olcum = new CreateOrEditOlcumDto()
                };
            }

            var viewModel = new CreateOrEditOlcumModalViewModel()
            {
                Olcum = getOlcumForEditOutput.Olcum,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewOlcumModal(int id)
        {
            var getOlcumForViewDto = await _olcumsAppService.GetOlcumForView(id);

            var model = new OlcumViewModel()
            {
                Olcum = getOlcumForViewDto.Olcum
            };

            return PartialView("_ViewOlcumModal", model);
        }

    }
}