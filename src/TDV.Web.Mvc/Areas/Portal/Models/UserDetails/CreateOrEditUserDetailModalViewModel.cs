using TDV.Authorization.Dtos;

using Abp.Extensions;

namespace TDV.Web.Areas.Portal.Models.UserDetails
{
    public class CreateOrEditUserDetailModalViewModel
    {
        public CreateOrEditUserDetailDto UserDetail { get; set; }

        public string UserName { get; set; }

        public string ContactDisplayProperty { get; set; }

        public bool IsEditMode => UserDetail.Id.HasValue;
    }
}