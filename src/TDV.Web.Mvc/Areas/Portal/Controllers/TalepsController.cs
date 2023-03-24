using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Taleps;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Rapor;
using TDV.Rapor.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Taleps)]
    public class TalepsController : TDVControllerBase
    {
        private readonly ITalepsAppService _talepsAppService;

        public TalepsController(ITalepsAppService talepsAppService)
        {
            _talepsAppService = talepsAppService;

        }

        public ActionResult Index()
        {
            var model = new TalepsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Taleps_Create, AppPermissions.Pages_Taleps_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetTalepForEditOutput getTalepForEditOutput;

            if (id.HasValue)
            {
                getTalepForEditOutput = await _talepsAppService.GetTalepForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getTalepForEditOutput = new GetTalepForEditOutput
                {
                    Talep = new CreateOrEditTalepDto()
                };
            }

            var viewModel = new CreateOrEditTalepModalViewModel()
            {
                Talep = getTalepForEditOutput.Talep,
                StokAdi = getTalepForEditOutput.StokAdi,
                TalepStokList = await _talepsAppService.GetAllStokForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewTalepModal(int id)
        {
            var getTalepForViewDto = await _talepsAppService.GetTalepForView(id);

            var model = new TalepViewModel()
            {
                Talep = getTalepForViewDto.Talep
                ,
                StokAdi = getTalepForViewDto.StokAdi

            };

            return PartialView("_ViewTalepModal", model);
        }

    }
}