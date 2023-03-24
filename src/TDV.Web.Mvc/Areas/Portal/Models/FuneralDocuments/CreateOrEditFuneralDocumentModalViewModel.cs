using TDV.Burial.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.FuneralDocuments
{
    public class CreateOrEditFuneralDocumentModalViewModel
    {
        public CreateOrEditFuneralDocumentDto FuneralDocument { get; set; }

        public string FuneralDisplayProperty { get; set; }

        public string PathFileName { get; set; }
        public string PathFileAcceptedTypes { get; set; }
        public bool IsEditMode => FuneralDocument.Id.HasValue;
    }
}