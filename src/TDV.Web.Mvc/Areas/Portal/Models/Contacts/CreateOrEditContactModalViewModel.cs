using TDV.Communication.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.Contacts
{
    public class CreateOrEditContactModalViewModel
    {
        public CreateOrEditContactDto Contact { get; set; }

        public bool IsEditMode => Contact.Id.HasValue;
    }
}