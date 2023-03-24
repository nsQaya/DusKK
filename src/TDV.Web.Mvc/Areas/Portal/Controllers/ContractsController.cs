using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.Contracts;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Payment;
using TDV.Payment.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_Contracts)]
    public class ContractsController : TDVControllerBase
    {
        private readonly IContractsAppService _contractsAppService;

        public ContractsController(IContractsAppService contractsAppService)
        {
            _contractsAppService = contractsAppService;

        }

        public ActionResult Index()
        {
            var model = new ContractsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Contracts_Create, AppPermissions.Pages_Contracts_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetContractForEditOutput getContractForEditOutput;

            if (id.HasValue)
            {
                getContractForEditOutput = await _contractsAppService.GetContractForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getContractForEditOutput = new GetContractForEditOutput
                {
                    Contract = new CreateOrEditContractDto()
                };
                getContractForEditOutput.Contract.StartDate = DateTime.Now;
                getContractForEditOutput.Contract.EndDate = DateTime.Now;
            }

            var viewModel = new CreateOrEditContractModalViewModel()
            {
                Contract = getContractForEditOutput.Contract,
                RegionName = getContractForEditOutput.RegionName,
                CompanyDisplayProperty = getContractForEditOutput.CompanyDisplayProperty,
                ContractRegionList = await _contractsAppService.GetAllRegionForTableDropdown(),
                ContractCompanyList = await _contractsAppService.GetAllCompanyForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewContractModal(int id)
        {
            var getContractForViewDto = await _contractsAppService.GetContractForView(id);

            var model = new ContractViewModel()
            {
                Contract = getContractForViewDto.Contract
                ,
                RegionName = getContractForViewDto.RegionName

                ,
                CompanyDisplayProperty = getContractForViewDto.CompanyDisplayProperty

            };

            return PartialView("_ViewContractModal", model);
        }

    }
}