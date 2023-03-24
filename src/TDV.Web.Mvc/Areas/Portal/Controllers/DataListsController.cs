using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.DataLists;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Constants;
using TDV.Constants.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_DataLists)]
    public class DataListsController : TDVControllerBase
    {
        private readonly IDataListsAppService _dataListsAppService;

        public DataListsController(IDataListsAppService dataListsAppService)
        {
            _dataListsAppService = dataListsAppService;

        }

        public ActionResult Index()
        {
            var model = new DataListsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DataLists_Create, AppPermissions.Pages_DataLists_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDataListForEditOutput getDataListForEditOutput;

            if (id.HasValue)
            {
                getDataListForEditOutput = await _dataListsAppService.GetDataListForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getDataListForEditOutput = new GetDataListForEditOutput
                {
                    DataList = new CreateOrEditDataListDto()
                };
            }

            var viewModel = new CreateOrEditDataListModalViewModel()
            {
                DataList = getDataListForEditOutput.DataList,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewDataListModal(int id)
        {
            var getDataListForViewDto = await _dataListsAppService.GetDataListForView(id);

            var model = new DataListViewModel()
            {
                DataList = getDataListForViewDto.DataList
            };

            return PartialView("_ViewDataListModal", model);
        }

    }
}