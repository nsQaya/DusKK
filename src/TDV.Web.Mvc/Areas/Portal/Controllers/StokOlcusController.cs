using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.StokOlcus;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Kalite;
using TDV.Kalite.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_StokOlcus)]
    public class StokOlcusController : TDVControllerBase
    {
        private readonly IStokOlcusAppService _stokOlcusAppService;

        public StokOlcusController(IStokOlcusAppService stokOlcusAppService)
        {
            _stokOlcusAppService = stokOlcusAppService;

        }

        public ActionResult Index()
        {
            var model = new StokOlcusViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_StokOlcus_Create, AppPermissions.Pages_StokOlcus_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetStokOlcuForEditOutput getStokOlcuForEditOutput;

            if (id.HasValue)
            {
                getStokOlcuForEditOutput = await _stokOlcusAppService.GetStokOlcuForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getStokOlcuForEditOutput = new GetStokOlcuForEditOutput
                {
                    StokOlcu = new CreateOrEditStokOlcuDto()
                };
            }

            var viewModel = new CreateOrEditStokOlcuModalViewModel()
            {
                StokOlcu = getStokOlcuForEditOutput.StokOlcu,
                StokAdi = getStokOlcuForEditOutput.StokAdi,
                OlcumOlcuTipi = getStokOlcuForEditOutput.OlcumOlcuTipi,
                StokOlcuOlcumList = await _stokOlcusAppService.GetAllOlcumForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewStokOlcuModal(int id)
        {
            var getStokOlcuForViewDto = await _stokOlcusAppService.GetStokOlcuForView(id);

            var model = new StokOlcuViewModel()
            {
                StokOlcu = getStokOlcuForViewDto.StokOlcu
                ,
                StokAdi = getStokOlcuForViewDto.StokAdi

                ,
                OlcumOlcuTipi = getStokOlcuForViewDto.OlcumOlcuTipi

            };

            return PartialView("_ViewStokOlcuModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_StokOlcus_Create, AppPermissions.Pages_StokOlcus_Edit)]
        public PartialViewResult StokLookupTableModal(int? id, string displayName)
        {
            var viewModel = new StokOlcuStokLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_StokOlcuStokLookupTableModal", viewModel);
        }

    }
}