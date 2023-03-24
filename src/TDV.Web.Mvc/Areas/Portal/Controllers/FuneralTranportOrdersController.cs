using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.FuneralTranportOrders;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Burial;
using TDV.Burial.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_FuneralTranportOrders)]
    public class FuneralTranportOrdersController : TDVControllerBase
    {
        private readonly IFuneralTranportOrdersAppService _funeralTranportOrdersAppService;

        public FuneralTranportOrdersController(IFuneralTranportOrdersAppService funeralTranportOrdersAppService)
        {
            _funeralTranportOrdersAppService = funeralTranportOrdersAppService;

        }

        public ActionResult Index()
        {
            var model = new FuneralTranportOrdersViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralTranportOrders_Create, AppPermissions.Pages_FuneralTranportOrders_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetFuneralTranportOrderForEditOutput getFuneralTranportOrderForEditOutput;

            if (id.HasValue)
            {
                getFuneralTranportOrderForEditOutput = await _funeralTranportOrdersAppService.GetFuneralTranportOrderForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFuneralTranportOrderForEditOutput = new GetFuneralTranportOrderForEditOutput
                {
                    FuneralTranportOrder = new CreateOrEditFuneralTranportOrderDto()
                };
                getFuneralTranportOrderForEditOutput.FuneralTranportOrder.StartDate = DateTime.Now;
                getFuneralTranportOrderForEditOutput.FuneralTranportOrder.OperationDate = DateTime.Now;
                getFuneralTranportOrderForEditOutput.FuneralTranportOrder.DeliveryDate = DateTime.Now;
                getFuneralTranportOrderForEditOutput.FuneralTranportOrder.EndDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditFuneralTranportOrderModalViewModel()
            {
                FuneralTranportOrder = getFuneralTranportOrderForEditOutput.FuneralTranportOrder,
                FuneralWorkOrderDetailDescription = getFuneralTranportOrderForEditOutput.FuneralWorkOrderDetailDescription,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFuneralTranportOrderModal(int id)
        {
            var getFuneralTranportOrderForViewDto = await _funeralTranportOrdersAppService.GetFuneralTranportOrderForView(id);

            var model = new FuneralTranportOrderViewModel()
            {
                FuneralTranportOrder = getFuneralTranportOrderForViewDto.FuneralTranportOrder
                ,
                FuneralWorkOrderDetailDescription = getFuneralTranportOrderForViewDto.FuneralWorkOrderDetailDescription

            };

            return PartialView("_ViewFuneralTranportOrderModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralTranportOrders_Create, AppPermissions.Pages_FuneralTranportOrders_Edit)]
        public PartialViewResult FuneralWorkOrderDetailLookupTableModal(int? id, string displayName)
        {
            var viewModel = new FuneralTranportOrderFuneralWorkOrderDetailLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralTranportOrderFuneralWorkOrderDetailLookupTableModal", viewModel);
        }

    }
}