using TDV.Communication.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.ContactDetails
{
    public class MasterDetailChild_Contact_CreateOrEditContactDetailModalViewModel
    {
        public CreateOrEditContactDetailDto ContactDetail { get; set; }

        public bool IsEditMode => ContactDetail.Id.HasValue;
    }
}