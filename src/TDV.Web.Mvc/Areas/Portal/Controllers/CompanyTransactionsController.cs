using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.CompanyTransactions;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Payment;
using TDV.Payment.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_CompanyTransactions)]
    public class CompanyTransactionsController : TDVControllerBase
    {
        private readonly ICompanyTransactionsAppService _companyTransactionsAppService;

        public CompanyTransactionsController(ICompanyTransactionsAppService companyTransactionsAppService)
        {
            _companyTransactionsAppService = companyTransactionsAppService;

        }

        public ActionResult Index()
        {
            var model = new CompanyTransactionsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_CompanyTransactions_Create, AppPermissions.Pages_CompanyTransactions_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetCompanyTransactionForEditOutput getCompanyTransactionForEditOutput;

            if (id.HasValue)
            {
                getCompanyTransactionForEditOutput = await _companyTransactionsAppService.GetCompanyTransactionForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getCompanyTransactionForEditOutput = new GetCompanyTransactionForEditOutput
                {
                    CompanyTransaction = new CreateOrEditCompanyTransactionDto()
                };
                getCompanyTransactionForEditOutput.CompanyTransaction.Date = DateTime.Now;
            }

            var viewModel = new CreateOrEditCompanyTransactionModalViewModel()
            {
                CompanyTransaction = getCompanyTransactionForEditOutput.CompanyTransaction,
                CompanyTaxAdministration = getCompanyTransactionForEditOutput.CompanyTaxAdministration,
                FuneralDisplayProperty = getCompanyTransactionForEditOutput.FuneralDisplayProperty,
                DataListValue = getCompanyTransactionForEditOutput.DataListValue,
                CurrencyCode = getCompanyTransactionForEditOutput.CurrencyCode,
                DataListValue2 = getCompanyTransactionForEditOutput.DataListValue2,
                CompanyTransactionCompanyList = await _companyTransactionsAppService.GetAllCompanyForTableDropdown(),
                CompanyTransactionFuneralList = await _companyTransactionsAppService.GetAllFuneralForTableDropdown(),
                CompanyTransactionDataListList = await _companyTransactionsAppService.GetAllDataListForTableDropdown(),
                CompanyTransactionCurrencyList = await _companyTransactionsAppService.GetAllCurrencyForTableDropdown(),

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewCompanyTransactionModal(int id)
        {
            var getCompanyTransactionForViewDto = await _companyTransactionsAppService.GetCompanyTransactionForView(id);

            var model = new CompanyTransactionViewModel()
            {
                CompanyTransaction = getCompanyTransactionForViewDto.CompanyTransaction
                ,
                CompanyTaxAdministration = getCompanyTransactionForViewDto.CompanyTaxAdministration

                ,
                FuneralDisplayProperty = getCompanyTransactionForViewDto.FuneralDisplayProperty

                ,
                DataListValue = getCompanyTransactionForViewDto.DataListValue

                ,
                CurrencyCode = getCompanyTransactionForViewDto.CurrencyCode

                ,
                DataListValue2 = getCompanyTransactionForViewDto.DataListValue2

            };

            return PartialView("_ViewCompanyTransactionModal", model);
        }

    }
}