using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TDV.Web.Areas.Portal.Models.FuneralDocuments;
using TDV.Web.Controllers;
using TDV.Authorization;
using TDV.Burial;
using TDV.Burial.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using TDV.Integration.Modules.Interfaces;
using Abp.UI;
using System.IO;
using System.Linq;
using TDV.Storage;
using Abp.Web.Models;
using TDV.Configuration;
using Microsoft.Extensions.Configuration;
using TDV.Integration.Modules.BlobStorage;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [AbpMvcAuthorize(AppPermissions.Pages_FuneralDocuments)]
    public class FuneralDocumentsController : TDVControllerBase
    {
        private readonly BlobStorageConfig _config;
        private readonly IFuneralDocumentsAppService _funeralDocumentsAppService;
        private readonly IBlobStorageModule _blobStorage;

        public FuneralDocumentsController(
            BlobStorageConfig configuration,
            IFuneralDocumentsAppService funeralDocumentsAppService,
            IBlobStorageModule blobStorage)
        {
            _funeralDocumentsAppService = funeralDocumentsAppService;
            _blobStorage = blobStorage;
            _config= configuration;
        }

        public ActionResult Index()
        {
            var model = new FuneralDocumentsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralDocuments_Create, AppPermissions.Pages_FuneralDocuments_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetFuneralDocumentForEditOutput getFuneralDocumentForEditOutput;

            if (id.HasValue)
            {
                getFuneralDocumentForEditOutput = await _funeralDocumentsAppService.GetFuneralDocumentForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getFuneralDocumentForEditOutput = new GetFuneralDocumentForEditOutput
                {
                    FuneralDocument = new CreateOrEditFuneralDocumentDto()
                };
            }

            var viewModel = new CreateOrEditFuneralDocumentModalViewModel()
            {
                FuneralDocument = getFuneralDocumentForEditOutput.FuneralDocument,
                FuneralDisplayProperty = getFuneralDocumentForEditOutput.FuneralDisplayProperty,
                PathFileName = getFuneralDocumentForEditOutput.PathFileName,
            };

            foreach (var PathAllowedFileType in _config.AllowedMimes)
            {
                viewModel.PathFileAcceptedTypes += "." + PathAllowedFileType + ",";
            }
            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewFuneralDocumentModal(int id)
        {
            var getFuneralDocumentForViewDto = await _funeralDocumentsAppService.GetFuneralDocumentForView(id);

            var model = new FuneralDocumentViewModel()
            {
                FuneralDocument = getFuneralDocumentForViewDto.FuneralDocument
                ,
                FuneralDisplayProperty = getFuneralDocumentForViewDto.FuneralDisplayProperty

            };

            return PartialView("_ViewFuneralDocumentModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_FuneralDocuments_Create, AppPermissions.Pages_FuneralDocuments_Edit)]
        public PartialViewResult FuneralLookupTableModal(int? id, string displayName)
        {
            var viewModel = new FuneralDocumentFuneralLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_FuneralDocumentFuneralLookupTableModal", viewModel);
        }
        public async Task<UploadedFileModel> UploadPathFile()
        {
            //Check input
            if (Request.Form.Files.Count == 0)
            {
                throw new UserFriendlyException(L("NoFileFoundError"));
            }

            var file = Request.Form.Files.First();
            if (file.Length > _config.MaxSize)
            {
                throw new UserFriendlyException(L("Warn_File_SizeLimit", _config.MaxSizeFriendly));
            }

            var fileType = Path.GetExtension(file.FileName).Substring(1);
            if (_config.AllowedMimes != null && _config.AllowedMimes.Length > 0 && !_config.AllowedMimes.Contains(fileType))
            {
                throw new UserFriendlyException(L("FileNotInAllowedFileTypes", _config.AllowedMimes));
            }
            var splitFilename = file.FileName.Split('.');

            var fileToken = await _blobStorage.Upload(file.OpenReadStream(), splitFilename[0] + "-{UUID}." + splitFilename[1]);
            return new UploadedFileModel()
            {
                Path = fileToken.Path,
                Guid = fileToken.Guid,
            };
        }

        public async Task<ActionResult> DownloadPathFile(string id)
        {
            var documentUrl = await _blobStorage.GetDocumentBlob(Guid.Parse(id).ToString("N"));
            return documentUrl == null ? NotFound() : Redirect(documentUrl);
        }
    }
}