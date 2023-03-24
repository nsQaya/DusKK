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
    public class FixedPriceDetailsController : TDVControllerBase
    {
        private readonly IFixedPriceDetailsAppService _fixedPriceDetailsAppService;

        public FixedPriceDetailsController(IFixedPriceDetailsAppService fixedPriceDetailsAppService)
        {
            _fixedPriceDetailsAppService = fixedPriceDetailsAppService;

        }

        public ActionResult Index()
        {
            var model = new FixedPriceDetailsViewModel
            {
                FilterText = ""
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

            var viewModel = new CreateOrEditFixedPriceDetailModalViewModel()
            {
                FixedPriceDetail = getFixedPriceDetailForEditOutput.FixedPriceDetail,
                FixedPriceName = getFixedPriceDetailForEditOutput.FixedPriceName,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFixedPriceDetailModal(int id)
        {
            var getFixedPriceDetailForViewDto = await _fixedPriceDetailsAppService.GetFixedPriceDetailForView(id);

            var model = new FixedPriceDetailViewModel()
            {
                FixedPriceDetail = getFixedPriceDetailForViewDto.FixedPriceDetail
                ,
                FixedPriceName = getFixedPriceDetailForViewDto.FixedPriceName

            };

            return PartialView("_ViewFixedPriceDetailModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FixedPriceDetails_Create, AppPermissions.Pages_FixedPriceDetails_Edit)]
        public PartialViewResult FixedPriceLookupTableModal(int? id, string displayName)
        {
            var viewModel = new FixedPriceDetailFixedPriceLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FixedPriceDetailFixedPriceLookupTableModal", viewModel);
        }

    }
}