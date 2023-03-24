using TDV.Corporation.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.CompanyContacts
{
    public class CreateOrEditCompanyContactModalViewModel
    {
        public CreateOrEditCompanyContactDto CompanyContact { get; set; }

        public string CompanyDisplayProperty { get; set; }

        public string ContactName { get; set; }

        public bool IsEditMode => CompanyContact.Id.HasValue;
    }
}