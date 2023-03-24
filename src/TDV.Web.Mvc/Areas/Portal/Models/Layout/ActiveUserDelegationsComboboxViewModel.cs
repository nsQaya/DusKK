using System.Collections.Generic;
using TDV.Authorization.Delegation;
using TDV.Authorization.Users.Delegation.Dto;

namespace TDV.Web.Areas.Portal.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }

        public List<UserDelegationDto> UserDelegations { get; set; }

        public string CssClass { get; set; }
    }
}
