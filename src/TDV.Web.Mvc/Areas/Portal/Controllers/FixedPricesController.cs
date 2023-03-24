using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.FixedPrices;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Payment;
using TDV.Payment.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_FixedPrices)]
    public class FixedPricesController : TDVControllerBase
    {
        private readonly IFixedPricesAppService _fixedPricesAppService;

        public FixedPricesController(IFixedPricesAppService fixedPricesAppService)
        {
            _fixedPricesAppService = fixedPricesAppService;

        }

        public ActionResult Index()
        {
            var model = new FixedPricesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FixedPrices_Create, AppPermissions.Pages_FixedPrices_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetFixedPriceForEditOutput getFixedPriceForEditOutput;

            if (id.HasValue)
            {
                getFixedPriceForEditOutput = await _fixedPricesAppService.GetFixedPriceForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFixedPriceForEditOutput = new GetFixedPriceForEditOutput
                {
                    FixedPrice = new CreateOrEditFixedPriceDto()
                };
            }

            var viewModel = new CreateOrEditFixedPriceModalViewModel()
            {
                FixedPrice = getFixedPriceForEditOutput.FixedPrice,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFixedPriceModal(int id)
        {
            var getFixedPriceForViewDto = await _fixedPricesAppService.GetFixedPriceForView(id);

            var model = new FixedPriceViewModel()
            {
                FixedPrice = getFixedPriceForViewDto.FixedPrice
            };

            return PartialView("_ViewFixedPriceModal", model);
        }

    }
}