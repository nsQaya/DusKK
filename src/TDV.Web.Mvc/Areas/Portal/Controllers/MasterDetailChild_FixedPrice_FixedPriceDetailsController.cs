using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.FixedPriceDetails;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Payment;
using TDV.Payment.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_FixedPriceDetails)]
    public class MasterDetailChild_FixedPrice_FixedPriceDetailsController : TDVControllerBase
    {
        private readonly IFixedPriceDetailsAppService _fixedPriceDetailsAppService;

        public MasterDetailChild_FixedPrice_FixedPriceDetailsController(IFixedPriceDetailsAppService fixedPriceDetailsAppService)
        {
            _fixedPriceDetailsAppService = fixedPriceDetailsAppService;
        }

        public ActionResult Index(int fixedPriceId)
        {
            var model = new MasterDetailChild_FixedPrice_FixedPriceDetailsViewModel
            {
                FilterText = "",
                FixedPriceId = fixedPriceId
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FixedPriceDetails_Create, AppPermissions.Pages_FixedPriceDetails_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetFixedPriceDetailForEditOutput getFixedPriceDetailForEditOutput;

            if (id.HasValue)
            {
                getFixedPriceDetailForEditOutput = await _fixedPriceDetailsAppService.GetFixedPriceDetailForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFixedPriceDetailForEditOutput = new GetFixedPriceDetailForEditOutput
                {
                    FixedPriceDetail = new CreateOrEditFixedPriceDetailDto()
                };
                getFixedPriceDetailForEditOutput.FixedPriceDetail.StartDate = DateTime.Now;
                getFixedPriceDetailForEditOutput.FixedPriceDetail.EndDate = DateTime.Now;
            }

            var viewModel = new MasterDetailChild_FixedPrice_CreateOrEditFixedPriceDetailModalViewModel()
            {
                FixedPriceDetail = getFixedPriceDetailForEditOutput.FixedPriceDetail,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFixedPriceDetailModal(int id)
        {
            var getFixedPriceDetailForViewDto = await _fixedPriceDetailsAppService.GetFixedPriceDetailForView(id);

            var model = new MasterDetailChild_FixedPrice_FixedPriceDetailViewModel()
            {
                FixedPriceDetail = getFixedPriceDetailForViewDto.FixedPriceDetail
            };

            return PartialView("_ViewFixedPriceDetailModal", model);
        }

    }
}