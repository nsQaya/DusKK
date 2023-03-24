using TDV.Communication.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.ContactDetails
{
    public class CreateOrEditContactDetailModalViewModel
    {
        public CreateOrEditContactDetailDto ContactDetail { get; set; }

        public string ContactIdentifyNo { get; set; }

        public bool IsEditMode => ContactDetail.Id.HasValue;
    }
}