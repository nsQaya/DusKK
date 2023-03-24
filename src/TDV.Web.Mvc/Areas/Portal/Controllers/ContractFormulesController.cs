using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.ContractFormules;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Payment;
using TDV.Payment.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_ContractFormules)]
    public class ContractFormulesController : TDVControllerBase
    {
        private readonly IContractFormulesAppService _contractFormulesAppService;

        public ContractFormulesController(IContractFormulesAppService contractFormulesAppService)
        {
            _contractFormulesAppService = contractFormulesAppService;

        }

        public ActionResult Index()
        {
            var model = new ContractFormulesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ContractFormules_Create, AppPermissions.Pages_ContractFormules_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetContractFormuleForEditOutput getContractFormuleForEditOutput;

            if (id.HasValue)
            {
                getContractFormuleForEditOutput = await _contractFormulesAppService.GetContractFormuleForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getContractFormuleForEditOutput = new GetContractFormuleForEditOutput
                {
                    ContractFormule = new CreateOrEditContractFormuleDto()
                };
            }

            var viewModel = new CreateOrEditContractFormuleModalViewModel()
            {
                ContractFormule = getContractFormuleForEditOutput.ContractFormule,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewContractFormuleModal(int id)
        {
            var getContractFormuleForViewDto = await _contractFormulesAppService.GetContractFormuleForView(id);

            var model = new ContractFormuleViewModel()
            {
                ContractFormule = getContractFormuleForViewDto.ContractFormule
            };

            return PartialView("_ViewContractFormuleModal", model);
        }

    }
}