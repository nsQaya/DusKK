using TDV.Burial.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Funerals
{
    public class CreateOrEditFuneralModalViewModel
    {
        public CreateOrEditFuneralDto Funeral { get; set; }

        public string FuneralTypeDescription { get; set; }

        public string ContactDisplayProperty { get; set; }

        public string CompanyDisplayProperty { get; set; }

        public string OrganizationUnitDisplayName { get; set; }

        public bool IsEditMode => Funeral.Id.HasValue;
    }
}